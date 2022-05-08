namespace InvoiceGenerator
{
    using System;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;

    /// <summary>
    /// Invoice generator engine.
    /// </summary>
    internal class GeneratorEngine : BaseClass, IGeneratorEngine
    {
        /// <summary>
        /// Common generation settings.
        /// </summary>
        private readonly InvoiceGeneratorSettings settings;

        /// <summary>
        /// Rupee converter.
        /// </summary>
        private readonly IAmountToWords rupeeWordConverter;

        /// <summary>
        /// Excel utilties
        /// </summary>
        private readonly IExcelUtilities excelUtilities;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratorEngine"/> class.
        /// </summary>
        /// <param name="_settingsProvider">Parameters for generation.</param>
        /// <param name="_excelUtilities">Excel utilities.</param>
        /// <param name="_rupeeWordConverter">Rupee to word converter.</param>
        /// <param name="_logger">Logger class.</param>
        public GeneratorEngine(
            ISettingsProvider _settingsProvider,
            IExcelUtilities _excelUtilities,
            IAmountToWords _rupeeWordConverter,
            ILogger _logger) : base(_logger)
        {
            this.excelUtilities = _excelUtilities;
            this.rupeeWordConverter = _rupeeWordConverter;
            this.settings = _settingsProvider.GetSettings();
        }

        /// <inheritdoc/>
        public void Execute(List<Appartement> appartments)
        {
            this.CloneTemplateFile();
            int invoiceNumber = settings.FirstInvoiceNumber;
            this.Logger.LogInfo(
                $"Generating all Excel invoices at {this.settings.ExcelOutputDirectory}");

            this.Logger.LogInfo(
                $"Generating all PDF invoices at {this.settings.PdfOutputDirectory}");

            int successfulInvoices = 0;
            foreach (Appartement appart in appartments)
            {
                try
                {
                    InvoiceParameters parameters = new InvoiceParameters(invoiceNumber, appart);

                    this.GenerateInvoiceForAppartement(parameters);
                    successfulInvoices++;
                }
                catch (Exception ex)
                {
                    this.Logger.LogWarning(
                        $"Unable to generate receipt for {appart.Id}. Retry operation.");

                    this.Logger.LogWarning(ex.ToString());
                }

                invoiceNumber++;
            }

            this.Logger.LogInfo($"Successfully created {successfulInvoices} invoices.");
            this.CleanupTemporaryTemplateFile();
        }

        /// <summary>
        /// Clone template file for working.
        /// </summary>
        /// <exception cref="Exception">Not found.</exception>
        private void CloneTemplateFile()
        {
            string errorMessage;
            if (!File.Exists(settings.TemplateFilePath))
            {
                errorMessage = $"File {this.settings.TemplateFilePath} does not exist";
                this.Logger.LogError(errorMessage);
                throw new Exception(errorMessage);
            }

            this.Logger.LogInfo(
                $"Creating copy of template file for working {this.settings.IntermediateTemplateFilePath}");

            XSSFWorkbook workbook = this.excelUtilities.OpenExcelWorkbook(
                this.settings.TemplateFilePath);

            this.excelUtilities.SaveAndCloseExcelFile(
                workbook,
                this.settings.IntermediateTemplateFilePath);
        }

        /// <summary>
        /// Clone template file for working.
        /// </summary>
        private void CleanupTemporaryTemplateFile()
        {
            if (File.Exists(settings.IntermediateTemplateFilePath))
            {
                this.Logger.LogInfo($"Deleting temp file {this.settings.IntermediateTemplateFilePath}");
            }

            File.Delete(settings.IntermediateTemplateFilePath);
        }

        /// <summary>
        /// Generate invoice for single appartement.
        /// </summary>
        /// <param name="parameters">Invoice parameters.</param>
        private void GenerateInvoiceForAppartement(InvoiceParameters parameters)
        {
            string invoiceFileName = parameters.GetInvoiceFileName();
            string excelInvoicePath = string.Format(
                "{0}\\{1}.xlsx",
                this.settings.ExcelOutputDirectory,
                invoiceFileName);

            this.GenerateExcelInvoice(parameters, excelInvoicePath);

            string pdfInvoicePath = string.Format(
                "{0}\\{1}.pdf",
                this.settings.PdfOutputDirectory,
                invoiceFileName);

            try
            {
                this.excelUtilities.PrintToPDF(excelInvoicePath, pdfInvoicePath);
            }
            catch (Exception ex)
            {
                this.Logger.LogError(
                    $"Unable to generate PDF invoice for {parameters.Details.Id}");

                this.Logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// Generate invoice for single appartement.
        /// </summary>
        /// <param name="parameters">Invoice parameters.</param>
        /// <param name="outFilePath">Excel invoice path.</param>
        private void GenerateExcelInvoice(
            InvoiceParameters parameters,
            string outFilePath)
        {
            XSSFWorkbook workbook = this.excelUtilities.OpenExcelWorkbook(
                this.settings.IntermediateTemplateFilePath);

            ISheet workSheet = workbook.GetSheetAt(0);

            // Common Details of owner/appartement.

            this.excelUtilities.SetCellValue(workSheet, 9, 2, parameters.Details.Owner);
            this.excelUtilities.SetCellValue(workSheet, 10, 2, parameters.Details.Occupant);
            this.excelUtilities.SetCellValue(workSheet, 11, 2, parameters.Details.Id);

            string invoiceNumber = string.Format(
                this.settings.InvoiceNumberFormat,
                parameters.InvoiceNumber);

            this.excelUtilities.SetCellValue(workSheet, 9, 5, invoiceNumber);

            // Calculation
            this.excelUtilities.SetCellValue(workSheet, 15, 4, parameters.Details.SquareFootage);
            this.excelUtilities.SetCellValue(workSheet, 20, 6, parameters.Details.Dues);

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
    }
}
