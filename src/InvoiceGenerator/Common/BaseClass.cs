namespace InvoiceGenerator
{
    /// <summary>
    /// Common utilities.
    /// </summary>
    public class BaseClass
    {
        /// <summary>
        /// Logger class.
        /// </summary>
        protected readonly ILogger Logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseClass"/> class.
        /// </summary>
        /// <param name="_logger">Logger class.</param>
        public BaseClass(ILogger _logger)
        {
            this.Logger = _logger;
        }
    }
}
