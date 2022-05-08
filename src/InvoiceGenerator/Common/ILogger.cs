namespace InvoiceGenerator
{
    /// <summary>
    /// Logger interface.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Log information.
        /// </summary>
        /// <param name="message">Message.</param>
        void LogInfo(string message);

        /// <summary>
        /// Log warning.
        /// </summary>
        /// <param name="message">Message.</param>
        void LogWarning(string message);

        /// <summary>
        /// Log error.
        /// </summary>
        /// <param name="message">Message.</param>
        void LogError(string message);
    }
}
