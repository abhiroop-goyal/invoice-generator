namespace InvoiceGenerator.Tests
{
    using System.Collections.Generic;

    internal class TestDataGenerator
    {
        public static List<AppartementPenalty> GetFakeAppartementDuesList()
        {
            return new List<AppartementPenalty>
            {
                new AppartementPenalty("1", 2000, 365),
                new AppartementPenalty("2", 2000, 0),
                new AppartementPenalty("4", 2000, 365)
            };
        }

        public static List<Appartement> GetFakeAppartementList()
        {
            return new List<Appartement>
            {
                new Appartement("1", "John", "Tenant", 2000),
                new Appartement("2", "Adam", "Tenant", 2000),
                new Appartement("3", "Rishi", "Tenant", 2000),
            };
        }
    }
}
