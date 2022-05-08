namespace InvoiceGenerator
{
    public interface IAmountToWords
    {
        /// <summary>
        /// Get amount in words.
        /// </summary>
        /// <param name="amount">Amount.</param>
        /// <returns>The string representation.</returns>
        string GetAmountInWords(double amount);
    }
}