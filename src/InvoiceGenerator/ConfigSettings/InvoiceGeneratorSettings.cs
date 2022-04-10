namespace InvoiceGenerator
{
    /// <summary>
    /// Invoice generation settings.
    /// </summary>
    internal class InvoiceGeneratorSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvoiceGeneratorSettings"/> class.
        /// </summary>
        /// <param name="outputDirectory"></param>
        public InvoiceGeneratorSettings(string outputDirectory)
        {
            this.ExcelOutputDirectory = outputDirectory + "/Excel";
            this.PdfOutputDirectory = outputDirectory + "/Pdf";

            if (!Directory.Exists(this.ExcelOutputDirectory))
            {
                Directory.CreateDirectory(this.ExcelOutputDirectory);
            }

            if (!Directory.Exists(this.PdfOutputDirectory))
            {
                Directory.CreateDirectory(this.PdfOutputDirectory);
            }
        }

        /// <summary>
        /// Gets or sets firsst receipt number.
        /// </summary>
        public int FirstInvoiceNumber { get; set; }

        /// <summary>
        /// Gets or sets the invoice number format.
        /// </summary>
        public string InvoiceNumberFormat { get; set; }

        /// <summary>
        /// Gets or sets invoice template file path.
        /// </summary>
        public string TemplateFilePath => "temp.xlsx";

        /// <summary>
        /// Gets or sets invoice template file path.
        /// </summary>
        public string OriginalTemplateFilePath { get; set; }

        /// <summary>
        /// Gets or sets appartement details file path.
        /// </summary>
        public string DetailsFilePath { get; set; }

        /// <summary>
        /// Gets or sets output directory for generated invoices in XLSX.
        /// </summary>
        public string ExcelOutputDirectory { get; }

        /// <summary>
        /// Gets or sets output directory for generated invoices in PDF.
        /// </summary>
        public string PdfOutputDirectory { get; }
    }
}
