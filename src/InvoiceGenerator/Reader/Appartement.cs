namespace InvoiceGenerator
{
    /// <summary>
    /// Appartement details.
    /// </summary>
    public class Appartement
    {
        public Appartement(
            string appNumber,
            string owner,
            string occupant,
            double squareFootage)
        {
            this.Id = appNumber;
            this.Owner = owner;
            this.Occupant = occupant;
            this.SquareFootage = squareFootage;
            this.Dues = 0;
        }

        /// <summary>
        /// Gets or sets Unique Appartement Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets owner name.
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// Gets or sets occupant type.
        /// </summary>
        public string Occupant { get; set; }

        /// <summary>
        /// Gets or sets square footage.
        /// </summary>
        public double SquareFootage { get; set; }

        /// <summary>
        /// Gets or sets past dues.
        /// </summary>
        public double Dues { get; internal set; }

        /// <summary>
        /// Sets <see cref="Appartement.Dues"/> object.
        /// </summary>
        /// <param name="due">Dues object.</param>
        public void SetPastDues(AppartementPenalty due, double interestRate)
        {
            this.Dues = due.PastDueAmount * due.NumberOfDays * interestRate / (1200);
        }
    }

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
