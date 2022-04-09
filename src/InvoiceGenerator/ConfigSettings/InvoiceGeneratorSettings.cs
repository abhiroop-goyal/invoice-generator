using System.Configuration;

namespace InvoiceGenerator
{
    /// <summary>
    /// Invoice generation settings.
    /// </summary>
    internal class InvoiceGeneratorSettings 
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
        /// Gets or sets output directory for generated invoices.
        /// </summary>
        public string OutputDirectory { get; set; }
    }
}
