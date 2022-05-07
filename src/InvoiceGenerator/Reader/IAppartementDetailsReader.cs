namespace InvoiceGenerator
{
    using System;
    using NPOI.SS.UserModel;

    interface IAppartementDetailsReader
    {
        /// <summary>
        /// Execute action.
        /// </summary>
        /// <returns>A list of appartements.</returns>
        /// <param name="inputFilePath">Input file path.</param>
        /// <exception cref="Exception">File not found.</exception>
        List<Appartement> Execute(string inputFilePath);

        /// <summary>
        /// Parses the excel row for appartement details.
        /// </summary>
        /// <param name="row">Excel row.</param>
        /// <returns>The details.</returns>
        Appartement? ParseRow(IRow row);
    }
}
