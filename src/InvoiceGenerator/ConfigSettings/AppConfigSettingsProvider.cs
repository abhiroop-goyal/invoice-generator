namespace InvoiceGenerator
{
    using System.Configuration;

    /// <summary>
    /// App config settings provider.
    /// </summary>
    internal class AppConfigSettingsProvider : BaseClass, ISettingsProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppConfigSettingsProvider"/> class.
        /// </summary>
        /// <param name="_logger">Logger class.</param>
        public AppConfigSettingsProvider(ILogger _logger) : base(_logger)
        {
        }

        /// <summary>
        /// Get Configuration settings.
        /// </summary>
        /// <returns>The settings.</returns>
        public InvoiceGeneratorSettings GetSettings()
        {
            var ConfigSettings = new InvoiceGeneratorSettings
            {
                OutputDirectory = ConfigurationSettings.AppSettings.Get("OutputDirectory"),
                OriginalTemplateFilePath = ConfigurationSettings.AppSettings.Get("InvoiceTemplate"),
                DetailsFilePath = ConfigurationSettings.AppSettings.Get("InputFile"),
                InvoiceNumberFormat = ConfigurationSettings.AppSettings.Get("InvoiceNumberFormat"),
            };

            int billNumber;

            if (!int.TryParse(
                ConfigurationSettings.AppSettings.Get("FirstInvoiceNumber"),
                out billNumber))
            {
                this.Logger.LogWarning(
                    "Unable to read FirstBillNumber from config. Starting with 1");
            }

            ConfigSettings.FirstInvoiceNumber = billNumber;
            return ConfigSettings;

        }
    }
}
