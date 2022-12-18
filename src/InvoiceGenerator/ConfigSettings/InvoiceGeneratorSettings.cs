using Newtonsoft.Json;

namespace InvoiceGenerator
{
    /// <summary>
    /// Invoice generation settings.
    /// </summary>
    public class InvoiceGeneratorSettings
    {
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
        [JsonIgnore]
        public string IntermediateTemplateFilePath => "temp.xlsx";

        /// <summary>
        /// Gets or sets invoice template file path.
        /// </summary>
        public string TemplateFilePath { get; set; }

        /// <summary>
        /// Gets or sets appartement details file path.
        /// </summary>
        public string AppartementDetailsFilePath { get; set; }

        /// <summary>
        /// Gets or sets file path for past dues.
        /// </summary>
        public string PastDuesFilePath { get; set; }

        /// <summary>
        /// Gets or sets per annum interest rate for past dues.
        /// Currently not being used anywhere.
        /// </summary>
        public int DuesInterestRatePerAnnum { get; set; }

        /// <summary>
        /// Gets or sets output directory for generated invoices in XLSX.
        /// </summary>
        [JsonProperty]
        public string OutputDirectory { get; set; }

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
