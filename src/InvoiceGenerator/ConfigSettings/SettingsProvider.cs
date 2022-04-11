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

    internal static class SettingsProvider
    {
        /// <summary>
        /// Settings provider.
        /// </summary>
        public static ISettingsProvider Instance 
            = new JSONConfigSettingsProvider(Logger.Instance);
    }
}
