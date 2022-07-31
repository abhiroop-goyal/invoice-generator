namespace InvoiceGenerator
{
    using System;

    public interface IAppartementDetailsReader
    {
        /// <summary>
        /// Execute action.
        /// </summary>
        /// <returns>A list of appartements.</returns>
        /// <param name="inputFilePath">Input file path.</param>
        /// <param name="duesFilePath">Dues file path.</param>
        /// <param name="interestRate">Rate of iterest.</param>
        /// <exception cref="Exception">File not found.</exception>
        public List<Appartement> Execute();

        /// <summary>
        /// Calculate summary of amount for all appartements..
        /// </summary>
        /// <returns>A list of appartements.</returns>
        /// <param name="dues">Past due amounts.</param>
        /// <param name="appartementDetails">Appartement details.</param>
        /// <param name="interestRate">Rate of iterest.</param>
        /// <exception cref="Exception">File not found.</exception>
        public List<Appartement> CalculateSummary(
            List<AppartementPenalty> dues,
            List<Appartement> appartementDetails);

        /// <summary>
        /// Get outstanding due amounts.
        /// </summary>
        /// <returns>A list of appartements with their past dues.</returns>
        /// <param name="duesFilePath">Input file path.</param>
        public List<AppartementPenalty> GetPastDues();

        /// <summary>
        /// Get all appartement details.
        /// </summary>
        /// <returns>A list of appartements.</returns>
        /// <param name="inputFilePath">Input file path.</param>
        public List<Appartement> GetAppartementDetails();
    }
}
