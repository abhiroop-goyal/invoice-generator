namespace InvoiceGenerator
{
    using System;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Invoice generator engine.
    /// </summary>
    public class GeneratorEngine : IGeneratorEngine
    {
        private readonly InvoiceGeneratorSettings settings;
        private readonly IReceiptGenerator receiptGenerator;
        private readonly ILogger Logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratorEngine"/> class.
        /// </summary>
        /// <param name="receiptGenerator">Receipt generator.</param>
        /// <param name="_logger">Logger class.</param>
        public GeneratorEngine(
            IOptions<InvoiceGeneratorSettings> _settingsProvider,
            IReceiptGenerator receiptGenerator,
            ILogger<GeneratorEngine> _logger)
        {
            this.Logger = _logger;
            this.receiptGenerator = receiptGenerator;
            this.settings = _settingsProvider.Value;
        }

        /// <inheritdoc/>
        public void Execute(List<Appartement> appartments)
        {
            this.receiptGenerator.Initialize();
            int invoiceNumber = settings.FirstInvoiceNumber;
            this.Logger.LogInformation(
                $"Generating all Excel invoices at {this.settings.ExcelOutputDirectory}");

            this.Logger.LogInformation(
                $"Generating all PDF invoices at {this.settings.PdfOutputDirectory}");

            int successfulInvoices = 0;
            int skippedInvoices = 0;
            foreach (Appartement appart in appartments)
            {
                if (!this.receiptGenerator.IsInvoiceRequired(appart))
                {
                    skippedInvoices++;
                    continue;
                }

                try
                {
                    InvoiceParameters parameters = new InvoiceParameters(invoiceNumber, appart);
                    this.GenerateInvoiceForAppartement(parameters);
                    successfulInvoices++;
                }
                catch (Exception ex)
                {
                    this.Logger.LogWarning(
                        $"Unable to generate receipt for {appart.Id}. Retry operation.");

                    this.Logger.LogWarning(ex.ToString());
                }

                invoiceNumber++;
            }

            this.Logger.LogInformation($"Successfully created {successfulInvoices} invoices.");
            this.receiptGenerator.Dispose();
        }

        /// <summary>
        /// Generate invoice for single appartement.
        /// </summary>
        /// <param name="parameters">Invoice parameters.</param>
        private void GenerateInvoiceForAppartement(InvoiceParameters parameters)
        {
            string invoiceFileName = parameters.GetInvoiceFileName();
            string excelInvoicePath = string.Format(
                "{0}\\{1}.xlsx",
                this.settings.ExcelOutputDirectory,
                invoiceFileName);

            this.receiptGenerator.GenerateExcelReceipt(parameters, excelInvoicePath);

            string pdfInvoicePath = string.Format(
                "{0}\\{1}.pdf",
                this.settings.PdfOutputDirectory,
                invoiceFileName);
            this.receiptGenerator.GeneratePdfReceipt(parameters.Details.Id, excelInvoicePath, pdfInvoicePath);
        }
    }
}
