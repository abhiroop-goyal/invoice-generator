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
            this.Logger = _logger;
            this.excelUtilities = excelUtilities;
            this.options = _options.Value;
            this.dynamicData = new Dictionary<string, AppartementDynamicData>();
            this.customCharges = new List<string>();
        }

        /// <inheritdoc/>
        public void EnrichDynamicData(Appartement data)
        {
            if (!this.dynamicData.ContainsKey(data.Id))
            {
                return;
            }

            var item = this.dynamicData[data.Id];

            data.Dues = item.PastDueAmount;
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
            if (row.Cells.Count <= 2)
            {
                this.Logger.LogInformation($"No custom charges found in Dues sheet");
                return;
            }

            for (int i = 2; i < row.Cells.Count; i++)
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

            if (cellsInRow < 2)
            {
                this.Logger.LogError($"Invalid input in Row {row.RowNum}");
                return null;
            }

            string id = this.excelUtilities.GetStringCellValue(row, 0);
            double dueAmount = this.excelUtilities.GetNumericalCellValue(row, 2);
            bool isAdvancePaid = string.Equals(
                this.excelUtilities.GetStringCellValue(row, 1),
                "True",
                StringComparison.OrdinalIgnoreCase);

            Dictionary<string, double> customChargeDict = new Dictionary<string, double>();
            for (int i = 0; i < customCharges && (i +2) < cellsInRow ; i++)
            {
                customChargeDict.Add(
                    this.customCharges[i], 
                    this.excelUtilities.GetNumericalCellValue(row, i + 2));
            }

            return new AppartementDynamicData(id, dueAmount, isAdvancePaid, customChargeDict);
        }
    }
}
