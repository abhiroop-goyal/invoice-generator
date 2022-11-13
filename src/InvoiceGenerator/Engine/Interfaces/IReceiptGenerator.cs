namespace InvoiceGenerator
{
    /// <summary>
    /// Receipt generator.
    /// </summary>
    public interface IReceiptGenerator
    {
        /// <summary>
        /// Dispose generator.
        /// </summary>
        void Dispose();

        /// <summary>
        /// Generate excel receipt.
        /// </summary>
        /// <param name="parameters">Report parameters.</param>
        /// <param name="outFilePath">Out file path.</param>
        void GenerateExcelReceipt(InvoiceParameters parameters, string outFilePath);

        /// <summary>
        /// Generate PDF receipt.
        /// </summary>
        /// <param name="appartementId">Appartement id.</param>
        /// <param name="excelInvoicePath">Input Excel invoice path.</param>
        /// <param name="pdfInvoicePath">PDF invoice path at which to be generated.</param>
        void GeneratePdfReceipt(string appartementId, string excelInvoicePath, string pdfInvoicePath);
        
        /// <summary>
        /// Initialize generator.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Check if invoice is required,
        /// </summary>
        /// <param name="appartementDetails">Appartement details.</param>
        /// <returns>True, if required.</returns>
        bool IsInvoiceRequired(Appartement appartementDetails);
    }
}