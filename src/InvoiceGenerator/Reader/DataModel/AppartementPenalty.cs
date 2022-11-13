namespace InvoiceGenerator
{
    /// <summary>
    /// Appartement details.
    /// </summary>
    public class AppartementPenalty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppartementPenalty"/> class.
        /// </summary>
        /// <param name="appNumber">Appartement number.</param>
        /// <param name="pastDueAmount">Past due amount.</param>
        /// <param name="numberOfDays">Number of days.</param>
        public AppartementPenalty(
            string appNumber,
            double pastDueAmount,
            double numberOfDays)
        {
            this.Id = appNumber;
            this.PastDueAmount = pastDueAmount;
            this.NumberOfDays = numberOfDays;
        }

        /// <summary>
        /// Gets or sets Unique Appartement Id.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets or sets Unique Appartement Id.
        /// </summary>
        public double PastDueAmount { get; }

        /// <summary>
        /// Gets or sets Unique Appartement Id.
        /// </summary>
        public double NumberOfDays { get; }
    }
}
