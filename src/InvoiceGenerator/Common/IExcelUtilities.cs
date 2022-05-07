namespace InvoiceGenerator
{
    using NPOI.XSSF.UserModel;

    public interface IExcelUtilities
    {
        /// <summary>
        /// Opens and Excel workbook.
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
        public void PrintToPDF(string inputFilePath, string outputPath);
    }
}
