using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Xunit;

namespace InvoiceGenerator.Tests
{
    public class AppartementDetailsReaderTest
    {
        private const string ApptFileName = "apps";
        private const string DuesFileName = "dues";
        private const int InterestRate = 10;

        public AppartementDetailsReaderTest()
        {
        }

        [Fact]
        public void CalculateDuesTest()
        {
            //AppartementDetailsReader reader;
            //var excelUtilsMock = new Mock<IExcelUtilities>();

            //excelUtilsMock
            //    .Setup(
            //        x => x.ParseExcelFile<Appartement>(
            //            It.IsAny<string>(),
            //            It.IsAny<Func<IRow, Appartement>>()))
            //    .Returns(TestDataGenerator.GetFakeAppartementList());

            //excelUtilsMock
            //    .Setup(
            //        x => x.ParseExcelFile<AppartementPenalty>(
            //            It.IsAny<string>(),
            //            It.IsAny<Func<IRow, AppartementPenalty>>()))
            //    .Returns(TestDataGenerator.GetFakeAppartementDuesList());

            //var settingsMock = new InvoiceGeneratorSettings()
            //{
            //    AppartementDetailsFilePath = "dummy",
            //    PastDuesFilePath = "dummy",
            //    DuesInterestRatePerAnnum = InterestRate
            //};

            //var optionsMock = new Mock<IOptions<InvoiceGeneratorSettings>>();
            //optionsMock.SetupGet(x => x.Value).Returns(settingsMock);

            //reader = new AppartementDetailsReader(
            //    new Mock<ILogger<AppartementDetailsReader>>().Object,
            //    optionsMock.Object,
            //    excelUtilsMock.Object);

            //List<Appartement> results = reader.Execute();

            //Dictionary<string, Appartement> dues = results.ToDictionary(
            //    item => item.Id,
            //    item => item);

            //Assert.IsTrue(results.Count == 3);
            //Assert.IsTrue(dues["3"].Dues == 0);
            //Assert.IsTrue(!dues.ContainsKey("4"));
            //Assert.IsTrue(dues["1"].Dues != 0);
        }
    }
}