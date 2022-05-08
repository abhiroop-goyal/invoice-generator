using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace InvoiceGenerator.Tests
{
    [TestClass]
    public class AppartementDetailsReaderTest
    {
        private const string ApptFileName = "apps";
        private const string DuesFileName = "dues";
        private const double InterestRate = 10;

        public AppartementDetailsReaderTest()
        {
        }

        [TestMethod]
        public void CalculateDuesTest()
        {
            AppartementDetailsReader reader;
            var excelUtilsMock = new Mock<IExcelUtilities>();

            excelUtilsMock
                .Setup(
                    x => x.ParseExcelFile<Appartement>(
                        It.IsAny<string>(),
                        It.IsAny<Func<IRow, Appartement>>()))
                .Returns(TestDataGenerator.GetFakeAppartementList());

            excelUtilsMock
                .Setup(
                    x => x.ParseExcelFile<AppartementPenalty>(
                        It.IsAny<string>(),
                        It.IsAny<Func<IRow, AppartementPenalty>>()))
                .Returns(TestDataGenerator.GetFakeAppartementDuesList());

            reader = new AppartementDetailsReader(
                new ConsoleLogger(),
                excelUtilsMock.Object);

            List<Appartement> results = reader.Execute(
                "Dummy",
                "Dummy",
                InterestRate);

            Dictionary<string, Appartement> dues = results.ToDictionary(
                item => item.Id,
                item => item);

            Assert.IsTrue(results.Count == 3);
            Assert.IsTrue(dues["2"].Dues == 0);
            Assert.IsTrue(dues["3"].Dues == 0);
            Assert.IsTrue(!dues.ContainsKey("4"));
            Assert.IsTrue(dues["1"].Dues != 0);
        }

        private static ILogger GetLogger()
        {
            return new ConsoleLogger();
        }
    }
}