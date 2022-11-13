namespace InvoiceGenerator
{
    using System;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;

    /// <summary>
    /// Invoice generator engine.
    /// </summary>
    public class ReceiptGenerator : IReceiptGenerator
    {
        private readonly InvoiceGeneratorSettings settings;
        private readonly IAmountToWords rupeeWordConverter;
        private readonly IExcelUtilities excelUtilities;
        private readonly ILogger Logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleGenerator"/> class.
        /// </summary>
        /// <param name="_settingsProvider">Parameters for generation.</param>
        /// <param name="_excelUtilities">Excel utilities.</param>
        /// <param name="_rupeeWordConverter">Rupee to word converter.</param>
        /// <param name="_logger">Logger class.</param>
        public ReceiptGenerator(
            IOptions<InvoiceGeneratorSettings> _settingsProvider,
            IExcelUtilities _excelUtilities,
            IAmountToWords _rupeeWordConverter,
            ILogger<ReceiptGenerator> _logger)
        {
            this.Logger = _logger;
            this.excelUtilities = _excelUtilities;
            this.rupeeWordConverter = _rupeeWordConverter;
            this.settings = _settingsProvider.Value;
        }

        /// <inheritdoc/>
        public void Initialize()
        {
            string errorMessage;
            if (!File.Exists(settings.TemplateFilePath))
            {
                errorMessage = $"File {this.settings.TemplateFilePath} does not exist";
                this.Logger.LogError(errorMessage);
                throw new Exception(errorMessage);
            }

            this.Logger.LogInformation(
                $"Creating copy of template file for working {this.settings.IntermediateTemplateFilePath}");

            XSSFWorkbook workbook = this.excelUtilities.OpenExcelWorkbook(
                this.settings.TemplateFilePath);

            this.excelUtilities.SaveAndCloseExcelFile(
                workbook,
                this.settings.IntermediateTemplateFilePath);

            Directory.CreateDirectory(this.settings.ExcelOutputDirectory);
            Directory.CreateDirectory(this.settings.PdfOutputDirectory);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (File.Exists(settings.IntermediateTemplateFilePath))
            {
                this.Logger.LogInformation($"Deleting temp file {this.settings.IntermediateTemplateFilePath}");
            }

            File.Delete(settings.IntermediateTemplateFilePath);
        }

        /// <inheritdoc/>
        public bool IsInvoiceRequired(Appartement appartementDetails)
        {
            bool skipInvoice = appartementDetails.ChargePerUnit == 0
                && appartementDetails.Dues == 0
                && appartementDetails.CustomCharges.Sum(item => item.Value) == 0;

            if (skipInvoice)
            {
                this.Logger.LogInformation(
                    "Skipping invoice generation for {id} as there are no billable items.",
                    appartementDetails.Id);
            }

            return !skipInvoice;
        }

        /// <inheritdoc/>
        public void GeneratePdfReceipt(
            string appartementId,
            string excelInvoicePath,
            string pdfInvoicePath)
        {
            try
            {
                this.excelUtilities.PrintToPDF(excelInvoicePath, pdfInvoicePath);
            }
            catch (Exception ex)
            {
                this.Logger.LogError(
                    $"Unable to generate PDF invoice for {appartementId}");

                this.Logger.LogError(ex.ToString());
            }
        }

        /// <inheritdoc/>
        public void GenerateExcelReceipt(
            InvoiceParameters parameters,
            string outFilePath)
        {
            XSSFWorkbook workbook = this.excelUtilities.OpenExcelWorkbook(
                this.settings.IntermediateTemplateFilePath);

            ISheet workSheet = workbook.GetSheetAt(0);

            this.SetHeaderDetails(workSheet, parameters);
            this.SetCommonItems(workSheet, parameters);
            this.SetCustomCharges(workSheet, parameters);

            // Changed pattern. Interest on dues comes via sheet.
            //this.excelUtilities.SetCellValue(workSheet, 21, 3, parameters.Details.NumberOfDaysForPenalty);

            // required as formulae are not auto-evaluated.
            XSSFFormulaEvaluator.EvaluateAllFormulaCells(workbook);

            // Set total value in words.
            double amount = this.excelUtilities.GetNumericalCellValue(workSheet, 24, 6);
            string amountInWords = string.Empty;
            try
            {
                amountInWords = this.rupeeWordConverter.GetAmountInWords(amount);
            }
            catch (Exception ex)
            {
                this.Logger.LogError($"Unable to generate word string for {amount}");
                this.Logger.LogError(ex.ToString());
            }

            this.excelUtilities.SetCellValue(workSheet, 25, 2, amountInWords);
            this.excelUtilities.SaveAndCloseExcelFile(workbook, outFilePath);
        }

        private void SetHeaderDetails(ISheet workSheet, InvoiceParameters parameters)
        {
            // Common Details of owner/appartement.

            this.excelUtilities.SetCellValue(workSheet, 9, 2, parameters.Details.Owner);
            this.excelUtilities.SetCellValue(workSheet, 10, 2, parameters.Details.Occupant);
            this.excelUtilities.SetCellValue(workSheet, 11, 2, parameters.Details.Id);

            string invoiceNumber = string.Format(
                this.settings.InvoiceNumberFormat,
                parameters.InvoiceNumber);

            this.excelUtilities.SetCellValue(workSheet, 9, 5, invoiceNumber);
        }

        private void SetCommonItems(ISheet workSheet, InvoiceParameters parameters)
        {
            this.excelUtilities.SetCellValue(workSheet, 15, 4, parameters.Details.SquareFootage);
            this.excelUtilities.SetCellValue(workSheet, 15, 5, parameters.Details.ChargePerUnit);
            this.excelUtilities.SetCellValue(workSheet, 18, 6, parameters.Details.Dues);
        }

        private void SetCustomCharges(ISheet workSheet, InvoiceParameters parameters)
        {
            int customChargeStartingRow = 16;
            foreach (var charge in parameters.Details.CustomCharges)
            {
                this.excelUtilities.SetCellValue(workSheet, customChargeStartingRow, 1, charge.Key);
                this.excelUtilities.SetCellValue(workSheet, customChargeStartingRow, 6, charge.Value);
                customChargeStartingRow++;
            }
        }
    }
}
