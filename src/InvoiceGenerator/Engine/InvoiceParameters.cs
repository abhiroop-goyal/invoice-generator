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
        /// Gets the invoice file name.
        /// </summary>
        /// <returns>Name of the file.</returns>
        public string GetInvoiceFileName()
        {
            return this.Details.Id
                .Replace("\\", string.Empty)
                .Replace("/", string.Empty);
        }
    }
}
