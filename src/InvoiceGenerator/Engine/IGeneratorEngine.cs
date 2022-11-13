namespace InvoiceGenerator
{
    using System;

    interface IGeneratorEngine
    {
        /// <summary>
        /// Execute generation.
        /// </summary>
        /// <param name="_apps">Appartement list.</param>
        /// <exception cref="Exception">Not found.</exception>
        void Execute(List<Appartement> _apps);

        /// <summary>
        /// Execute generation.
        /// </summary>
        /// <param name="appartementDetails">Appartement details.</param>
        bool IsInvoiceRequired(Appartement appartementDetails);
    }
}
