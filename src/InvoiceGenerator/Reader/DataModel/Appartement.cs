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
            double squareFootage,
            double chargePerUnit)
        {
            this.Id = appNumber;
            this.Owner = owner;
            this.Occupant = occupant;
            this.SquareFootage = squareFootage;
            this.Dues = 0;
            this.NumberOfDaysForPenalty = 0;
            this.ChargePerUnit = chargePerUnit;
            this.CustomCharges = new Dictionary<string, double>();
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
        /// Gets or sets charge per square footage.
        /// </summary>
        public double ChargePerUnit { get; set; }

        /// <summary>
        /// Gets or sets past dues.
        /// </summary>
        public double Dues { get; set; }

        /// <summary>
        /// Gets or sets past dues.
        /// </summary>
        public double NumberOfDaysForPenalty { get; internal set; }

        /// <summary>
        /// Gets or sets Unique Appartement Id.
        /// </summary>
        public Dictionary<string, double> CustomCharges { get; }
    }
}
