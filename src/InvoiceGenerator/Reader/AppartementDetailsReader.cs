namespace InvoiceGenerator
{
    using System;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Reader for details.
    /// </summary>
    internal class AppartementDetailsReader : BaseClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppartementDetailsReader"/> class.
        /// </summary>
        /// <param name="_logger">Logger class.</param>
        public AppartementDetailsReader(ILogger _logger) : base(_logger)
        {
        }

        /// <summary>
        /// Execute action.
        /// </summary>
        /// <returns>A list of appartements.</returns>
        /// <param name="inputFilePath">Input file path.</param>
        /// <exception cref="Exception">File not found.</exception>
        public List<Appartement> Execute(string inputFilePath)
        {
            List<Appartement> appartments = new List<Appartement>();
            this.Logger.LogInfo("Reading data of appartements from excel file.");

            IWorkbook? workbook = new ExcelUtilities(this.Logger).OpenExcelWorkbook(inputFilePath);

            try
            {
                ISheet workSheet = workbook.GetSheetAt(0);
                var rowIterator = workSheet.GetRowEnumerator();
                rowIterator.MoveNext();
                while (rowIterator.MoveNext())
                {
                    IRow row = (IRow)rowIterator.Current;
                    var app = this.ParseRow(row);
                    if (app != null)
                    {
                        appartments.Add(app);
                    }
                }

                this.Logger.LogInfo(
                    $"Successfully read data for {appartments.Count} appartements.");
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex.ToString());
                throw;
            }
            finally
            {
                workbook?.Close();
            }

            return appartments;
        }

        /// <summary>
        /// Parses the excel row for appartement details.
        /// </summary>
        /// <param name="row">Excel row.</param>
        /// <returns>The details.</returns>
        public Appartement? ParseRow(IRow row)
        {
            Appartement? app = null;
            if (row.Cells.Count >= 4)
            {
                #pragma warning disable CS8604 // Possible null reference argument.
                
                app = new Appartement(
                    appNumber: row.Cells[0].ToString(),
                    owner: row.Cells[1].ToString(),
                    occupant: row.Cells[2].ToString(),
                    squareFootage: row.Cells[3].NumericCellValue,
                    dues: row.Cells[4].NumericCellValue);
                
                #pragma warning restore CS8604 // Possible null reference argument.
            }
            else
            {
                this.Logger.LogWarning($"Invalid input in Row {row.RowNum}");
            }

            return app;
        }
    }
}
