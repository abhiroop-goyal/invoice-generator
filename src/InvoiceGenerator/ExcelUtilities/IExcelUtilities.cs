namespace InvoiceGenerator
{
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;

    public interface IExcelUtilities
    {
        /// <summary>
        /// Opens an Excel workbook.
        /// </summary>
        /// <param name="inputFileName">Input file name.</param>
        /// <returns>The workbook.</returns>
        /// <exception cref="Exception">File or sheets not found.</exception>
        XSSFWorkbook OpenExcelWorkbook(string inputFileName);

        /// <summary>
        /// Save excel file.
        /// </summary>
        /// <param name="workbook">Excel workbook.</param>
        /// <param name="outFilePath">Output path.</param
        void SaveAndCloseExcelFile(XSSFWorkbook workbook, string outFilePath);

        /// <summary>
        /// Print an excel file to PDF.
        /// </summary>
        /// <param name="inputFilePath">Input file path.</param>
        /// <param name="outputPath">Output file path.</param>
        void PrintToPDF(string inputFilePath, string outputPath);

        /// <summary>
        /// Parse rows of an excel file.
        /// </summary>
        /// <returns>A list of appartements.</returns>
        /// <param name="inputFilePath">Input file path.</param>
        /// <param name="parsingFunction">Parsing function.</param>
        /// <exception cref="Exception">File not found.</exception>
        List<T> ParseExcelFile<T>(
            string inputFilePath, 
            Func<IRow, T> parsingFunction,
            Action<IRow>? firstRowCallback = null);

        /// <summary>
        /// Set cell value.
        /// </summary>
        /// <param name="workSheet">Excel worksheet.</param>
        /// <param name="rowNumber">Row number.</param>
        /// <param name="columnNumber">Column number.</param>
        /// <param name="value">Cell value to fill.</param>
        void SetCellValue(
            ISheet workSheet,
            int rowNumber,
            int columnNumber,
            double value);

        /// <summary>
        /// Set cell value.
        /// </summary>
        /// <param name="workSheet">Excel worksheet.</param>
        /// <param name="rowNumber">Row number.</param>
        /// <param name="columnNumber">Column number.</param>
        /// <param name="value">Cell value to fill.</param>
        void SetCellValue(
            ISheet workSheet,
            int rowNumber,
            int columnNumber,
            string value);

        /// <summary>
        /// Get cell value.
        /// </summary>
        /// <param name="workSheet">Excel worksheet.</param>
        /// <param name="rowNumber">Row number.</param>
        /// <param name="columnNumber">Column number.</param>
        /// <param name="value">Cell value to fill.</param>
        /// <returns>The cell value.</returns>
        double GetNumericalCellValue(
            ISheet workSheet,
            int rowNumber,
            int columnNumber);

        /// <summary>
        /// Get cell value.
        /// </summary>
        /// <param name="workSheet">Excel worksheet.</param>
        /// <param name="rowNumber">Row number.</param>
        /// <param name="columnNumber">Column number.</param>
        /// <param name="value">Cell value to fill.</param>
        /// <returns>The cell value.</returns>
        string GetStringCellValue(
            ISheet workSheet,
            int rowNumber,
            int columnNumber);

        /// <summary>
        /// Get cell value.
        /// </summary>
        /// <param name="row">Row object.</param>
        /// <param name="columnNumber">Column number.</param>
        /// <returns>The double value.</returns>
        double GetNumericalCellValue(
            IRow row,
            int columnNumber);

        /// <summary>
        /// Get cell value.
        /// </summary>
        /// <param name="row">Row object.</param>
        /// <param name="columnNumber">Column number.</param>
        /// <returns>The string value.</returns>
        string GetStringCellValue(
            IRow row,
            int columnNumber);
    }
}
