using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace InvoiceGenerator.Tests.Engine
{
    /// <summary>
    /// Tests for <see cref="GeneratorEngine"/>.
    /// </summary>
    public class GeneratorEngineTests
    {
        private readonly Mock<IReceiptGenerator> mockGen;

        public GeneratorEngineTests()
        {
            this.mockGen = new Mock<IReceiptGenerator>();
        }

        [Fact]
        public void Execute_ReceiptGenerationNotRequired_NotGenerated()
        {
            var engine = this.GetEngine(mock => 
            {
                mock
                    .Setup(x => x.IsInvoiceRequired(It.IsAny<Appartement>()))
                    .Returns(false);
            });

            engine.Execute(new List<Appartement> { this.GetFakeAppartement() });
            this.mockGen.Verify(x => x.GenerateExcelReceipt(It.IsAny<InvoiceParameters>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Execute_ReceiptGenerationRequired_Generated()
        {
            var engine = this.GetEngine(mock =>
            {
                mock
                    .Setup(x => x.IsInvoiceRequired(It.IsAny<Appartement>()))
                    .Returns(true);
            });

            engine.Execute(new List<Appartement> { this.GetFakeAppartement() });
            this.mockGen.Verify(x => x.GenerateExcelReceipt(It.IsAny<InvoiceParameters>(), It.IsAny<string>()), Times.Once);
            this.mockGen.Verify(x => x.GenerateExcelReceipt(It.IsAny<InvoiceParameters>(), It.IsAny<string>()), Times.Once);
        }


        [Fact]
        public void Execute_ReceiptGenerationRequired_SameExcelFileUsed()
        {
            var app = this.GetFakeAppartement();
            string excelFilePath = string.Empty;
            var engine = this.GetEngine(mock =>
            {
                mock
                    .Setup(x => x.IsInvoiceRequired(It.IsAny<Appartement>()))
                    .Returns(true);
                
                mock
                    .Setup(x => x.GenerateExcelReceipt(It.IsAny<InvoiceParameters>(), It.IsAny<string>()))
                    .Callback<InvoiceParameters, string>((a, b) => 
                    {
                        excelFilePath = b;
                    });
            });

            engine.Execute(new List<Appartement> { app });
            this.mockGen.Verify(x => x.GeneratePdfReceipt(app.Id, excelFilePath, It.IsAny<string>()), Times.Once);
        }

        private GeneratorEngine GetEngine(Action<Mock<IReceiptGenerator>> setupCallback)
        {
            setupCallback(mockGen);

            return new GeneratorEngine(
                Options.Create(new InvoiceGeneratorSettings()),
                mockGen.Object,
                new Mock<ILogger<GeneratorEngine>>().Object);

        }

        private Appartement GetFakeAppartement()
        {
            return new Appartement(this.GetRandomString(), this.GetRandomString(), this.GetRandomString(), 1, 0);
        }

        private string GetRandomString()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
