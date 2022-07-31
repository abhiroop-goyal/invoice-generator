namespace InvoiceGenerator
{
    using System;
    using Microsoft.Extensions.Logging;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="AppartementDetailsReader"/> class.
        /// </summary>
        /// <param name="_logger">Logger class.</param>
        public AppartementDetailsReader(
            ILogger<AppartementDetailsReader> _logger,
            IExcelUtilities excelUtilities)
        {
            this.Logger = _logger;
            this.excelUtilities = excelUtilities;
        }

        /// <inheritdoc/>
        public List<Appartement> Execute(
            string inputFilePath,
            string duesFilePath,
            double interestRate)
        {
            return this.CalculateSummary(
                this.GetPastDues(duesFilePath),
                this.GetAppartementDetails(inputFilePath),
                interestRate);
        }

        /// <inheritdoc/>
        public List<Appartement> CalculateSummary(
            List<AppartementPenalty> dues,
            List<Appartement> appartementDetails,
            double interestRate)
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
        public List<AppartementPenalty> GetPastDues(string duesFilePath)
        {
            this.Logger.LogInformation("Reading data for past dues.");
            return this.excelUtilities.ParseExcelFile(
                duesFilePath,
                this.ParseDuesRow);
        }

        /// <inheritdoc/>
        public List<Appartement> GetAppartementDetails(string inputFilePath)
        {
            this.Logger.LogInformation("Reading data of appartements from excel file.");
            return this.excelUtilities.ParseExcelFile<Appartement>(
                inputFilePath,
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
