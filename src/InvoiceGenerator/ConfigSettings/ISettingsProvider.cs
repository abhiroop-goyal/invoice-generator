namespace InvoiceGenerator
{
    interface ISettingsProvider
    {
        /// <summary>
        /// Get Configuration settings.
        /// </summary>
        /// <returns>The settings.</returns>
        InvoiceGeneratorSettings GetSettings();
    }
}