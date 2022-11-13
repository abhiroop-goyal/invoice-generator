namespace InvoiceGenerator
{
    /// <summary>
    /// Appartement details.
    /// </summary>
    public class AppartementDynamicData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppartementPenalty"/> class.
        /// </summary>
        /// <param name="appNumber">Appartement number.</param>
        /// <param name="pastDueAmount">Past due amount.</param>
        /// <param name="isAdvancePaid">Is Advance paid.</param>
        /// <param name="customCharges">Custom charge.</param>
        public AppartementDynamicData(
            string appNumber,
            double pastDueAmount,
            bool isAdvancePaid,
            Dictionary<string,double> customCharges)
        {
            this.Id = appNumber;
            this.PastDueAmount = pastDueAmount;
            this.CustomCharges = customCharges;
            this.IsAdvancePaid = isAdvancePaid;
        }

        /// <summary>
        /// Gets or sets Unique Appartement Id.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets a value indicating if advance is paid.
        /// </summary>
        public bool IsAdvancePaid { get; }

        /// <summary>
        /// Gets or sets Unique Appartement Id.
        /// </summary>
        public double PastDueAmount { get; }

        /// <summary>
        /// Gets or sets Unique Appartement Id.
        /// </summary>
        public Dictionary<string, double> CustomCharges { get; }
    }
}
