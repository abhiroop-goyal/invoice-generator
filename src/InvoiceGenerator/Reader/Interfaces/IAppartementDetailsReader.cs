namespace InvoiceGenerator
{
    using System;

    public interface IAppartementDetailsReader
    {
        /// <summary>
        /// Execute action.
        /// </summary>
        /// <returns>A list of appartements.</returns>
        /// <exception cref="Exception">File not found.</exception>
        public List<Appartement> Execute();
    }
}
