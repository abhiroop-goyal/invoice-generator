namespace InvoiceGenerator
{
    using System;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Reader for details.
    /// </summary>
    public class StaticAppartementDetailsReader : IStaticDetailsReader
    {
        private readonly IExcelUtilities excelUtilities;
        private readonly ILogger Logger;
        private readonly InvoiceGeneratorSettings options;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppartementDetailsReader"/> class.
        /// </summary>
        /// <param name="_logger">Logger class.</param>
        public StaticAppartementDetailsReader(
            ILogger<StaticAppartementDetailsReader> _logger,
            IOptions<InvoiceGeneratorSettings> _options,
            IExcelUtilities excelUtilities)
        {
            this.Logger = _logger;
            this.excelUtilities = excelUtilities;
            this.options = _options.Value;
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
            if (row.Cells.Count < 5)
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
    }
}
