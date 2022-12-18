namespace InvoiceGenerator
{
    using System;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Reader for details.
    /// </summary>
    public class DuesCalculatorReaderV2 : IDynamicDetailsReader
    {
        private enum WellKnownColumns
        {
            Id,
            Dues,
            InterestOnDues,
            IsAdvancePaid
        }

        private readonly Dictionary<WellKnownColumns, int> wellKnownColumnIndex;
        private readonly IExcelUtilities excelUtilities;
        private readonly ILogger Logger;
        private readonly InvoiceGeneratorSettings options;
        private readonly Dictionary<string, AppartementDynamicData> dynamicData;
        private readonly List<string> customCharges = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AppartementDetailsReader"/> class.
        /// </summary>
        /// <param name="_logger">Logger class.</param>
        public DuesCalculatorReaderV2(
            ILogger<DuesCalculatorReaderV2> _logger,
            IOptions<InvoiceGeneratorSettings> _options,
            IExcelUtilities excelUtilities)
        {
            this.wellKnownColumnIndex = new Dictionary<WellKnownColumns, int>
            {
                { WellKnownColumns.Id, 0 },
                { WellKnownColumns.Dues, 1 },
                { WellKnownColumns.InterestOnDues, 2 },
                { WellKnownColumns.IsAdvancePaid, 3 },
            };

            this.Logger = _logger;
            this.excelUtilities = excelUtilities;
            this.options = _options.Value;
            this.dynamicData = new Dictionary<string, AppartementDynamicData>();
            this.customCharges = new List<string>();
        }

        private int PermanentColumnCount => this.wellKnownColumnIndex.Count;

        /// <inheritdoc/>
        public void EnrichDynamicData(Appartement data)
        {
            if (!this.dynamicData.ContainsKey(data.Id))
            {
                return;
            }

            var item = this.dynamicData[data.Id];

            data.Dues = item.PastDueAmount;
            data.InterestOnDues = item.InterestOnPastDueAmount;
            item.CustomCharges.Keys
                .ToList()
                .ForEach(key => data.CustomCharges.Add(key, item.CustomCharges[key]));

            if (item.IsAdvancePaid)
            {
                data.ChargePerUnit = 0;
            }
        }

        /// <inheritdoc/>
        public void PrepareData()
        {
            this.Logger.LogInformation("Reading data for past dues.");
            var dynamicData = this.excelUtilities.ParseExcelFile(
                this.options.PastDuesFilePath,
                this.ParseDuesRow,
                this.SetCustomChargesIfExist);

            dynamicData.ForEach(item => this.dynamicData.Add(item.Id, item));
        }

        /// <summary>
        /// Sets strings for custom charges from header row.
        /// </summary>
        /// <param name="row">Header row.</param>
        private void SetCustomChargesIfExist(IRow row)
        {
            if (row.Cells.Count <= this.PermanentColumnCount)
            {
                this.Logger.LogInformation($"No custom charges found in Dues sheet");
                return;
            }

            for (int i = this.PermanentColumnCount; i < row.Cells.Count; i++)
            {
                string header = this.excelUtilities.GetStringCellValue(row, i);
                this.Logger.LogInformation("Found custom charge {charge}", header);
                this.customCharges.Add(header);
            }
        }

        /// <summary>
        /// Parses the excel row for appartement details.
        /// </summary>
        /// <param name="row">Excel row.</param>
        /// <returns>The details.</returns>
        private AppartementDynamicData ParseDuesRow(IRow row)
        {
            int cellsInRow = row.Cells.Count;
            int customCharges = this.customCharges.Count();

            if (cellsInRow < this.PermanentColumnCount)
            {
                this.Logger.LogError($"Invalid input in Row {row.RowNum}");
                return null;
            }

            string id = this.excelUtilities.GetStringCellValue(
                row,
                this.wellKnownColumnIndex[WellKnownColumns.Id]);

            double dueAmount = this.excelUtilities.GetNumericalCellValue(
                row,
                this.wellKnownColumnIndex[WellKnownColumns.Dues]);

            double interest = this.excelUtilities.GetNumericalCellValue(
                row,
                this.wellKnownColumnIndex[WellKnownColumns.InterestOnDues]);

            bool isAdvancePaid = string.Equals(
                this.excelUtilities.GetStringCellValue(
                    row,
                    this.wellKnownColumnIndex[WellKnownColumns.IsAdvancePaid]),
                "Yes",
                StringComparison.OrdinalIgnoreCase);

            Dictionary<string, double> customChargeDict = new Dictionary<string, double>();
            for (int i = 0; i < customCharges && (i + this.PermanentColumnCount) < cellsInRow; i++)
            {
                // always adding charge for 0.0 case as well.
                double charge = this.excelUtilities.GetNumericalCellValue(row, i + this.PermanentColumnCount);
                customChargeDict.Add(this.customCharges[i], charge);
            }

            return new AppartementDynamicData(id, dueAmount, interest, isAdvancePaid, customChargeDict);
        }
    }
}
