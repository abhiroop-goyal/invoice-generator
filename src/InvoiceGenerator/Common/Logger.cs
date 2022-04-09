namespace InvoiceGenerator
{
    using System;

    /// <summary>
    /// Logger class.
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Internal instance.
        /// </summary>
        public static ILogger Instance = new ConsoleLogger();

        /// <summary>
        /// Console logger.
        /// </summary>
        private class ConsoleLogger : ILogger
        {
            /// <inheritdoc/>
            public void LogInfo(string message)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(message);
            }

            /// <inheritdoc/>
            public void LogWarning(string message)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(message);
            }

            /// <inheritdoc/>
            public void LogError(string message)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
            }
        }
    }

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
