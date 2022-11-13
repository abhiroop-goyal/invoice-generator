namespace InvoiceGenerator
{
    public interface IDynamicDetailsReader
    {
        /// <summary>
        /// Get outstanding due amounts.
        /// </summary>
        /// <returns>A list of appartements with their past dues.</returns>
        public void PrepareData();

        /// <summary>
        /// Enrich with dynamic data.
        /// </summary>
        public void EnrichDynamicData(Appartement data);
    }
}
