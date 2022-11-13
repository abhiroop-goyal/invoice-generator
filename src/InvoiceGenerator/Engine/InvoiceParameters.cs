namespace InvoiceGenerator
{
    /// <summary>
    /// Parameters to generate an invoice.
    /// </summary>
    public class InvoiceParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvoiceParameters"/> class.
        /// </summary>
        /// <param name="invoiceNumber">Invoice number.</param>
        /// <param name="details">Appartement details.</param>
        public InvoiceParameters(int invoiceNumber, Appartement details)
        {
            this.InvoiceNumber = invoiceNumber;
            this.Details = details;
        }

        /// <summary>
        /// Gets or sets the Invoice Number.
        /// </summary>
        public int InvoiceNumber { get; }

        /// <summary>
        /// Gets or sets the details of the appartement.
        /// </summary>
        public Appartement Details { get; }

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
