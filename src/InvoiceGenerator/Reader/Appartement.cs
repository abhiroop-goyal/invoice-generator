namespace InvoiceGenerator
{
    /// <summary>
    /// Appartement details.
    /// </summary>
    internal class Appartement
    {
        public Appartement(
            string appNumber, 
            string owner, 
            string occupant, 
            double squareFootage, 
            double dues)
        {
            this.Id = appNumber;
            this.Owner = owner;
            this.Occupant = occupant;
            this.SquareFootage = squareFootage;
            this.Dues = dues;
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
        public double Dues { get; set; }
    }
}
