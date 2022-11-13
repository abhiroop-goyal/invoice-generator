namespace InvoiceGenerator
{
    using System;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Reader for details.
    /// </summary>
    public class DuesCalculatorReader : IDynamicDetailsReader
    {
        private readonly IExcelUtilities excelUtilities;
        private readonly ILogger Logger;
        private readonly InvoiceGeneratorSettings options;
        private readonly Dictionary<string, AppartementPenalty> penalties;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppartementDetailsReader"/> class.
        /// </summary>
        /// <param name="_logger">Logger class.</param>
        public DuesCalculatorReader(
            ILogger<DuesCalculatorReader> _logger,
            IOptions<InvoiceGeneratorSettings> _options,
            IExcelUtilities excelUtilities)
        {
            this.Logger = _logger;
            this.excelUtilities = excelUtilities;
            this.options = _options.Value;
            this.penalties = new Dictionary<string, AppartementPenalty>();
        }

        /// <inheritdoc/>
        public void EnrichDynamicData(Appartement data)
        {
            if (!this.penalties.ContainsKey(data.Id))
            {
                return;
            }

            var item = this.penalties[data.Id];

            data.Dues = item.PastDueAmount;
            data.NumberOfDaysForPenalty = item.NumberOfDays;
        }

        /// <inheritdoc/>
        public void PrepareData()
        {
            this.Logger.LogInformation("Reading data for past dues.");
            var dynamicData = this.excelUtilities.ParseExcelFile(
                this.options.PastDuesFilePath,
                this.ParseDuesRow);

            dynamicData.ForEach(item => this.penalties.Add(item.Id, item));
        }

        /// <summary>
        /// Parses the excel row for appartement details.
        /// </summary>
        /// <param name="row">Excel row.</param>
        /// <returns>The details.</returns>
        private AppartementPenalty ParseDuesRow(IRow row)
        {
            if (row.Cells.Count < 3)
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
