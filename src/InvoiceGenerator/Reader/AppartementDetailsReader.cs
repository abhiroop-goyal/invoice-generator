namespace InvoiceGenerator
{
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Reader for details.
    /// </summary>
    public class AppartementDetailsReader : IAppartementDetailsReader
    {
        private readonly IStaticDetailsReader staticDetailsReader;
        private readonly IDynamicDetailsReader dynamicDetailsReader;
        private readonly ILogger Logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppartementDetailsReader"/> class.
        /// </summary>
        /// <param name="_logger">Logger class.</param>
        public AppartementDetailsReader(
            ILogger<AppartementDetailsReader> _logger,
            IStaticDetailsReader staticDetailsReader,
            IDynamicDetailsReader dynamicDetailsReader)
        {
            this.Logger = _logger;
            this.staticDetailsReader = staticDetailsReader;
            this.dynamicDetailsReader = dynamicDetailsReader;
        }

        /// <inheritdoc/>
        public List<Appartement> Execute()
        {
            this.dynamicDetailsReader.PrepareData();
            List<Appartement> appartementInfo = this.staticDetailsReader.GetAppartementDetails();
            foreach (Appartement app in appartementInfo)
            {
                this.dynamicDetailsReader.EnrichDynamicData(app);
            }

            return appartementInfo;
        }
    }
}
