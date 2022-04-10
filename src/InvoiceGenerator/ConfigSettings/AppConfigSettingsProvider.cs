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
            if (!int.TryParse(
                ConfigurationManager.AppSettings.Get("FirstInvoiceNumber"),
                out int firstInvoiceNumber))
            {
                this.Logger.LogWarning(
                    "Unable to parse FirstBillNumber from config. Starting with 1.");
            }

            var settings = new InvoiceGeneratorSettings(
                outputDirectory: this.GetConfigurationSetting("OutputDirectory"),
                originalTemplateFilePath: this.GetConfigurationSetting("InvoiceTemplate"),
                detailsFilePath: this.GetConfigurationSetting("InputFile"),
                invoiceNumberFormat: this.GetConfigurationSetting("InvoiceNumberFormat"),
                firstInvoiceNumber: firstInvoiceNumber);

            return settings;
        }

        /// <summary>
        /// Gets configuration setting.
        /// </summary>
        /// <param name="settingName">Setting name.</param>
        /// <returns>Value of setting.</returns>
        /// <exception cref="Exception">Not found exception.</exception>
        private string GetConfigurationSetting(string settingName)
        {
            string? settingValue = ConfigurationManager.AppSettings.Get(settingName);
            if (!string.IsNullOrWhiteSpace(settingValue))
            {
                return settingValue;
            }
            else
            {
                string message = $"Unable to find {settingName} in config settings.";
                this.Logger.LogError(message);
                throw new Exception(message);
            }
        }
    }
}
