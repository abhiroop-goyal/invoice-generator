namespace InvoiceGenerator
{
    using NPOI.XSSF.UserModel;

    /// <summary>
    /// Excel utilities.
    /// </summary>
    internal class ExcelUtilities : BaseClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelUtilities"/> class.
        /// </summary>
        /// <param name="_logger">Logger class.</param>
        public ExcelUtilities(ILogger _logger) : base(_logger)
        {
        }

        /// <summary>
        /// Opens and Excel workbook.
        /// </summary>
        /// <param name="inputFileName">Input file name.</param>
        /// <returns>The workbook.</returns>
        /// <exception cref="Exception">File or sheets not found.</exception>
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
        /// <param name="outFilePath">Output path.</param
        public void SaveAndCloseExcelFile(XSSFWorkbook workbook, string outFilePath)
        {
            FileStream fileStream = File.Create(outFilePath);
            workbook.Write(fileStream);
            fileStream.Close();
            workbook.Close();
        }
    }
}
