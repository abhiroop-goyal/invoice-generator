using Newtonsoft.Json;

namespace InvoiceGenerator
{
    /// <summary>
    /// Invoice generation settings.
    /// </summary>
    internal class InvoiceGeneratorSettings
    {
        public InvoiceGeneratorSettings(
            int firstInvoiceNumber,
            int interestRateForPastDues,
            string invoiceNumberFormat,
            string originalTemplateFilePath,
            string appartementDetailsFilePath,
            string outputDirectory,
            string pastDuesFilePath)
        {
            this.DuesInterestRatePerAnnum = interestRateForPastDues;
            this.FirstInvoiceNumber = firstInvoiceNumber;
            this.InvoiceNumberFormat = invoiceNumberFormat;
            this.TemplateFilePath = originalTemplateFilePath;
            this.AppartementDetailsFilePath = appartementDetailsFilePath;
            this.OutputDirectory = outputDirectory;
            this.PastDuesFilePath = pastDuesFilePath;
            this.CreateDirectories();
        }

        /// <summary>
        /// Create output directories if they dont exist.
        /// </summary>
        private void CreateDirectories() 
        { 
            if (!Directory.Exists(this.OutputDirectory))
            {
                Directory.CreateDirectory(this.OutputDirectory);
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
        [JsonIgnore]
        public string IntermediateTemplateFilePath => "temp.xlsx";

        /// <summary>
        /// Gets or sets invoice template file path.
        /// </summary>
        public string TemplateFilePath { get; }

        /// <summary>
        /// Gets or sets appartement details file path.
        /// </summary>
        public string AppartementDetailsFilePath { get; }

        /// <summary>
        /// Gets or sets file path for past dues.
        /// </summary>
        public string PastDuesFilePath { get; }

        /// <summary>
        /// Gets or sets per annum interest rate for past dues.
        /// Currently not being used anywhere.
        /// </summary>
        public int DuesInterestRatePerAnnum { get; }

        /// <summary>
        /// Gets or sets output directory for generated invoices in XLSX.
        /// </summary>
        [JsonProperty]
        public string OutputDirectory  { get; }

        /// <summary>
        /// Gets or sets output directory for generated invoices in XLSX.
        /// </summary>
        [JsonIgnore]
        public string ExcelOutputDirectory => string.Concat(this.OutputDirectory, "/Excel");

        /// <summary>
        /// Gets or sets output directory for generated invoices in PDF.
        /// </summary>
        [JsonIgnore]
        public string PdfOutputDirectory => string.Concat(this.OutputDirectory, "/Pdf");
    }
}
