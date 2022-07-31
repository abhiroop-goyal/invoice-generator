namespace InvoiceGenerator
{
    using System.Collections;
    using System.Drawing.Printing;
    using Microsoft.Extensions.Logging;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;

    /// <summary>
    /// Excel utilities.
    /// </summary>
    public class ExcelUtilities : IExcelUtilities
    {
        /// <summary>
        /// Logger interface.
        /// </summary>
        private readonly ILogger<ExcelUtilities> Logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelUtilities"/> class.
        /// </summary>
        /// <param name="_logger">Logger class.</param>
        public ExcelUtilities(ILogger<ExcelUtilities> _logger)
        {
            this.Logger = _logger;
        }

        /// <inheritdoc />
        public XSSFWorkbook OpenExcelWorkbook(string inputFileName)
        {
            string errorMessage;
            if (!File.Exists(inputFileName))
            {
                errorMessage = $"File {inputFileName} does not exist";
                this.Logger.LogError(errorMessage);
                throw new Exception(errorMessage);
            }

            XSSFWorkbook workbook = new XSSFWorkbook(inputFileName);
            if (workbook.NumberOfSheets == 0)
            {
                errorMessage = "No sheets found in excel.";
                this.Logger.LogError(errorMessage);
                throw new Exception(errorMessage);
            }

            return workbook;
        }

        /// <summary>
        /// Save excel file.
        /// </summary>
        /// <param name="workbook">Excel workbook.</param>
        /// <param name="outFilePath">Output path.</param>
        public void SaveAndCloseExcelFile(XSSFWorkbook workbook, string outFilePath)
        {
            FileStream fileStream = File.Create(outFilePath);
            workbook.Write(fileStream);
            fileStream.Close();
            workbook.Close();
        }

        /// <summary>
        /// Print an excel file to PDF.
        /// </summary>
        /// <param name="inputFilePath">Input file path.</param>
        /// <param name="outputPath">Output file path.</param>
        public void PrintToPDF(string inputFilePath, string outputPath)
        {
            Spire.Xls.Workbook workbook = new Spire.Xls.Workbook();
            workbook.LoadFromFile(inputFilePath);
            PrintDocument pd = workbook.PrintDocument;

#pragma warning disable CA1416 // Validate platform compatibility

            pd.PrinterSettings.PrinterName = "Microsoft Print to PDF";

            // tell the object this document will print to file
            pd.PrinterSettings.PrintToFile = true;
            // set the filename to whatever you like (full path)
            pd.PrinterSettings.PrintFileName = Path.Combine(outputPath);
            pd.Print();

#pragma warning restore CA1416 // Validate platform compatibility
        }

        /// <summary>
        /// Set cell value.
        /// </summary>
        /// <param name="workSheet">Excel worksheet.</param>
        /// <param name="rowNumber">Row number.</param>
        /// <param name="columnNumber">Column number.</param>
        /// <param name="value">Cell value to fill.</param>
        public void SetCellValue(
            ISheet workSheet,
            int rowNumber,
            int columnNumber,
            double value)
        {
            ICell cell = this.GetCell(
                workSheet,
                rowNumber,
                columnNumber,
                createIfNotExists: true);

            cell.SetCellValue(value);
        }

        /// <summary>
        /// Set cell value.
        /// </summary>
        /// <param name="workSheet">Excel worksheet.</param>
        /// <param name="rowNumber">Row number.</param>
        /// <param name="columnNumber">Column number.</param>
        /// <param name="value">Cell value to fill.</param>
        public void SetCellValue(
            ISheet workSheet,
            int rowNumber,
            int columnNumber,
            string value)
        {
            ICell cell = this.GetCell(
                workSheet,
                rowNumber,
                columnNumber,
                createIfNotExists: true);

            cell.SetCellValue(value);
        }

        /// <summary>
        /// Set cell value.
        /// </summary>
        /// <param name="workSheet">Excel worksheet.</param>
        /// <param name="rowNumber">Row number.</param>
        /// <param name="columnNumber">Column number.</param>
        /// <param name="value">Cell value to fill.</param>
        public double GetNumericalCellValue(
            ISheet workSheet,
            int rowNumber,
            int columnNumber)
        {
            ICell cell = this.GetCell(
                workSheet,
                rowNumber,
                columnNumber,
                createIfNotExists: false);

            if (cell == null)
            {
                return default(double);
            }

            return cell.NumericCellValue;
        }

        /// <summary>
        /// Get cell value.
        /// </summary>
        /// <param name="row">Row object.</param>
        /// <param name="columnNumber">Column number.</param>
        /// <returns>The double value.</returns>
        public double GetNumericalCellValue(
            IRow row,
            int columnNumber)
        {
            ICell cell = row.GetCell(columnNumber);

            if (cell == null)
            {
                return default(double);
            }

            return cell.NumericCellValue;
        }

        /// <summary>
        /// Set cell value.
        /// </summary>
        /// <param name="workSheet">Excel worksheet.</param>
        /// <param name="rowNumber">Row number.</param>
        /// <param name="columnNumber">Column number.</param>
        /// <param name="value">Cell value to fill.</param>
        public string GetStringCellValue(
            ISheet workSheet,
            int rowNumber,
            int columnNumber)
        {
            ICell cell = this.GetCell(
                workSheet,
                rowNumber,
                columnNumber,
                createIfNotExists: false);

            if (cell == null)
            {
                return string.Empty;
            }

            return cell.StringCellValue;
        }

        /// <summary>
        /// Get cell value.
        /// </summary>
        /// <param name="row">Row object.</param>
        /// <param name="columnNumber">Column number.</param>
        /// <returns>The string value.</returns>
        public string GetStringCellValue(
            IRow row,
            int columnNumber)
        {
            ICell cell = row.GetCell(columnNumber);

            if (cell == null)
            {
                return string.Empty;
            }

            return cell.StringCellValue;
        }

        /// <summary>
        /// Parse rows of an excel file.
        /// </summary>
        /// <returns>A list of appartements.</returns>
        /// <param name="inputFilePath">Input file path.</param>
        /// <param name="parsingFunction">Parsing function.</param>
        /// <exception cref="Exception">File not found.</exception>
        public List<T> ParseExcelFile<T>(string inputFilePath, Func<IRow, T> parsingFunction)
        {
            List<T> items = new List<T>();
            IWorkbook? workbook = this.OpenExcelWorkbook(inputFilePath);

            try
            {
                ISheet workSheet = workbook.GetSheetAt(0);
                IEnumerator rowIterator = workSheet.GetRowEnumerator();
                // skip header
                rowIterator.MoveNext();

                while (rowIterator.MoveNext())
                {
                    IRow row = (IRow)rowIterator.Current;
                    T parsedRow = parsingFunction(row);
                    if (parsedRow != null)
                    {
                        items.Add(parsedRow);
                    }
                }

                this.Logger.LogInformation(
                    $"Successfully read data for {items.Count} rows.");
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

            return items;
        }

        /// <summary>
        /// Set cell value.
        /// </summary>
        /// <param name="workSheet">Excel worksheet.</param>
        /// <param name="rowNumber">Row number.</param>
        /// <param name="columnNumber">Column number.</param>
        /// <param name="value">Cell value to fill.</param>
        private ICell GetCell(
            ISheet workSheet,
            int rowNumber,
            int columnNumber,
            bool createIfNotExists)
        {
            IRow row = workSheet.GetRow(rowNumber);
            if (row == null)
            {
                this.Logger.LogWarning($"Row {rowNumber} does not exist");
                if (!createIfNotExists)
                {
                    return null;
                }

                row = workSheet.CreateRow(rowNumber);
                this.Logger.LogInformation($"Row {rowNumber} successfully created.");
            }

            if (row.Cells.Count <= columnNumber)
            {
                this.Logger.LogWarning($"Cell {columnNumber} does not exist");
                if (!createIfNotExists)
                {
                    return null;
                }

                for (int colIndex = row.Cells.Count; colIndex <= columnNumber; colIndex++)
                {
                    row.CreateCell(colIndex);
                }

                this.Logger.LogInformation($"Cell {columnNumber} successfully created.");
            }

            return row.GetCell(columnNumber);
        }
    }
}
