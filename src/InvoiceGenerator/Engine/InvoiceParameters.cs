namespace InvoiceGenerator
{
    /// <summary>
    /// Parameters to generate an invoice.
    /// </summary>
    internal class InvoiceParameters
    {
        /// <summary>
        /// Gets or sets the Invoice Number.
        /// </summary>
        public int InvoiceNumber { get; set; }

        /// <summary>
        /// Gets or sets the details of the appartement.
        /// </summary>
        public Appartement Details { get; set; }

        /// <summary>
        /// Get invoice path.
        /// </summary>
        /// <param name="outputDirectory">Output directory.</param>
        /// <returns>The path of the invoice.</returns>
        public string GetInvoicePath(string outputDirectory)
        {
            return string.Format(
                "{0}\\{1}",
                outputDirectory,
                this.GetInvoiceFileName());
        }

        /// <summary>
        /// Gets the invoice file name.
        /// </summary>
        /// <returns>Name of the file.</returns>
        private string GetInvoiceFileName()
        {
            string fileName = this.Details.Id
                .Replace("\\", string.Empty)
                .Replace("/", string.Empty);

            return fileName + ".xlsx";
        }
    }
}
