namespace InvoiceGenerator
{
    /// <summary>
    /// Invoice generation settings.
    /// </summary>
    internal class InvoiceGeneratorSettings
    {
        public InvoiceGeneratorSettings(
            int firstInvoiceNumber,
            string invoiceNumberFormat,
            string originalTemplateFilePath,
            string detailsFilePath,
            string outputDirectory)
        {
            this.FirstInvoiceNumber = firstInvoiceNumber;
            this.InvoiceNumberFormat = invoiceNumberFormat;
            this.OriginalTemplateFilePath = originalTemplateFilePath;
            this.DetailsFilePath = detailsFilePath;

            this.ExcelOutputDirectory = outputDirectory + "/Excel";
            this.PdfOutputDirectory = outputDirectory + "/Pdf";

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

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
        public int FirstInvoiceNumber { get; }

        /// <summary>
        /// Gets or sets the invoice number format.
        /// </summary>
        public string InvoiceNumberFormat { get; }

        /// <summary>
        /// Gets or sets invoice template file path.
        /// </summary>
        public string TemplateFilePath => "temp.xlsx";

        /// <summary>
        /// Gets or sets invoice template file path.
        /// </summary>
        public string OriginalTemplateFilePath { get; }

        /// <summary>
        /// Gets or sets appartement details file path.
        /// </summary>
        public string DetailsFilePath { get; }

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
