namespace InvoiceGenerator
{
    using Newtonsoft.Json;
    using System.Configuration;

    /// <summary>
    /// JSON File config settings provider.
    /// </summary>
    internal class JSONConfigSettingsProvider : BaseClass, ISettingsProvider
    {
        private Dictionary<string, string>? items;

        /// <summary>
        /// Initializes a new instance of the <see cref="JSONConfigSettingsProvider"/> class.
        /// </summary>
        /// <param name="_logger">Logger class.</param>
        public JSONConfigSettingsProvider(ILogger _logger) : base(_logger)
        {
        }

        /// <summary>
        /// Get Configuration settings.
        /// </summary>
        /// <returns>The settings.</returns>
        public InvoiceGeneratorSettings GetSettings()
        {
            string filePath = Environment.GetCommandLineArgs()[1];
            if (!File.Exists(filePath))
            {
                string message = $"Configuration file {filePath}, does not exist. " +
                    $"Make sure that the file exists before proceeding";
                
                this.Logger.LogError(message);
                throw new Exception(message);
            }

            string content = File.ReadAllText(filePath);
            items = JsonConvert.DeserializeObject<Dictionary<string, string>>(
                content);

            if (!int.TryParse(
                this.GetConfigurationSetting("FirstInvoiceNumber"),
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
            if (this.items != null
                && this.items.TryGetValue(settingName, out string settingValue)
                && !string.IsNullOrWhiteSpace(settingValue))
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
