namespace InvoiceGenerator
{
    using System;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Reader for details.
    /// </summary>
    public class AppartementDetailsReader : IAppartementDetailsReader
    {
        /// <summary>
        /// Excel utilities.
        /// </summary>
        private IExcelUtilities excelUtilities;

        /// <summary>
        /// Logger interface.
        /// </summary>
        private readonly ILogger<AppartementDetailsReader> Logger;

        private InvoiceGeneratorSettings options;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppartementDetailsReader"/> class.
        /// </summary>
        /// <param name="_logger">Logger class.</param>
        public AppartementDetailsReader(
            ILogger<AppartementDetailsReader> _logger,
            IOptions<InvoiceGeneratorSettings> _options,
            IExcelUtilities excelUtilities)
        {
            this.Logger = _logger;
            this.excelUtilities = excelUtilities;
            this.options = _options.Value;
        }

        /// <inheritdoc/>
        public List<Appartement> Execute()
        {
            return this.CalculateSummary(
                this.GetPastDues(),
                this.GetAppartementDetails());
        }

        /// <inheritdoc/>
        public List<Appartement> CalculateSummary(
            List<AppartementPenalty> dues,
            List<Appartement> appartementDetails)
        {
            Dictionary<string, Appartement> appartementInfo =
                appartementDetails
                    .Where(item => item != null)
                    .ToDictionary(item => item.Id, item => item);

            foreach (AppartementPenalty dueRecord in dues)
            {
                if (appartementInfo.ContainsKey(dueRecord.Id))
                {
                    appartementInfo[dueRecord.Id].SetPastDues(dueRecord);
                }
                else
                {
                    this.Logger.LogWarning($"Due record found for unknown appartement {dueRecord.Id}");
                }
            }

            return appartementInfo.Select(item => item.Value).ToList();
        }

        /// <inheritdoc/>
        public List<AppartementPenalty> GetPastDues()
        {
            this.Logger.LogInformation("Reading data for past dues.");
            return this.excelUtilities.ParseExcelFile(
                this.options.PastDuesFilePath,
                this.ParseDuesRow);
        }

        /// <inheritdoc/>
        public List<Appartement> GetAppartementDetails()
        {
            this.Logger.LogInformation("Reading data of appartements from excel file.");
            return this.excelUtilities.ParseExcelFile<Appartement>(
                this.options.AppartementDetailsFilePath,
                this.ParseAppartementDetailsRow);
        }

        /// <summary>
        /// Parses the excel row for appartement details.
        /// </summary>
        /// <param name="row">Excel row.</param>
        /// <returns>The details.</returns>
        private Appartement ParseAppartementDetailsRow(IRow row)
        {
            if (row.Cells.Count < 3)
            {
                this.Logger.LogWarning($"Invalid input in Row {row.RowNum}");
                return null;
            }

            return new Appartement(
                appNumber: this.excelUtilities.GetStringCellValue(row, 0),
                owner: this.excelUtilities.GetStringCellValue(row, 1),
                occupant: this.excelUtilities.GetStringCellValue(row, 2),
                squareFootage: this.excelUtilities.GetNumericalCellValue(row, 3),
                chargePerUnit: this.excelUtilities.GetNumericalCellValue(row, 4));
        }

        /// <summary>
        /// Parses the excel row for appartement details.
        /// </summary>
        /// <param name="row">Excel row.</param>
        /// <returns>The details.</returns>
        private AppartementPenalty ParseDuesRow(IRow row)
        {
            if (row.Cells.Count < 2)
            {
                this.Logger.LogWarning($"Invalid input in Row {row.RowNum}");
                return null;
            }

            string id = this.excelUtilities.GetStringCellValue(row, 0);
            double dueAmount = this.excelUtilities.GetNumericalCellValue(row, 1);
            double elapsedDays = this.excelUtilities.GetNumericalCellValue(row, 2);
            return new AppartementPenalty(id, dueAmount, elapsedDays);
        }
    }
}
