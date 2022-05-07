namespace InvoiceGenerator
{
    using System.Drawing.Printing;
    using NPOI.XSSF.UserModel;
    using Spire.Xls;

    /// <summary>
    /// Excel utilities.
    /// </summary>
    public class ExcelUtilities : BaseClass, IExcelUtilities
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

        /// <summary>
        /// Print an excel file to PDF.
        /// </summary>
        /// <param name="inputFilePath">Input file path.</param>
        /// <param name="outputPath">Output file path.</param>
        public void PrintToPDF(string inputFilePath, string outputPath)
        {
            Workbook workbook = new Workbook();
            workbook.LoadFromFile(inputFilePath);
            PrintDocument pd = workbook.PrintDocument;
            pd.PrinterSettings.PrinterName = "Microsoft Print to PDF";

            // tell the object this document will print to file
            pd.PrinterSettings.PrintToFile = true;
            // set the filename to whatever you like (full path)
            pd.PrinterSettings.PrintFileName = Path.Combine(outputPath);
            pd.Print();
        }
    }
}
