namespace InvoiceGenerator
{
    /// <summary>
    /// Appartement details.
    /// </summary>
    internal class Appartement
    {
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
