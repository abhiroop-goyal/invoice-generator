namespace InvoiceGenerator
{
    public interface IStaticDetailsReader
    {
        /// <summary>
        /// Get all appartement details.
        /// </summary>
        /// <returns>A list of appartements.</returns>
        public List<Appartement> GetAppartementDetails();
    }
}
