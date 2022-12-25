using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace InvoiceGenerator.Tests.Engine
{
    /// <summary>
    /// Tests for <see cref="ReceiptGenerator"/>.
    /// </summary>
    public class ReceiptGeneratorTests
    {
        private readonly Mock<IExcelUtilities> mockGen;

        public ReceiptGeneratorTests()
        {
            this.mockGen = new Mock<IExcelUtilities>();
        }

        [Fact]
        public void IsInvoiceRequired_NoPendingPaymentFound_NotRequired()
        {
            var engine = this.GetGenerator(mock => { });
            var app = this.GetFakeAppartement();
            app.ChargePerUnit = 0;
            app.CustomCharges.Clear();
            app.Dues = 0;

            Assert.False(engine.IsInvoiceRequired(app));
        }

        [Fact]
        public void IsInvoiceRequired_ZeroCustomCharge_NotRequired()
        {
            var engine = this.GetGenerator(mock => { });
            var app = this.GetFakeAppartement();
            app.ChargePerUnit = 0;
            app.CustomCharges.Clear();
            app.CustomCharges.Add(this.GetRandomString(), 0);
            app.Dues = 0;

            Assert.False(engine.IsInvoiceRequired(app));
        }

        [Fact]
        public void IsInvoiceRequired_DuePending_Required()
        {
            var engine = this.GetGenerator(mock => { });
            var app = this.GetFakeAppartement();
            app.ChargePerUnit = 0;
            app.CustomCharges.Clear();
            app.Dues = 1000;

            Assert.True(engine.IsInvoiceRequired(app));
        }

        [Fact]
        public void IsInvoiceRequired_CustomCharge_Required()
        {
            var engine = this.GetGenerator(mock => { });
            var app = this.GetFakeAppartement();
            app.ChargePerUnit = 0;
            app.CustomCharges.Add(this.GetRandomString(), 45);
            app.Dues = 0;

            Assert.True(engine.IsInvoiceRequired(app));
        }

        [Fact]
        public void IsInvoiceRequired_CommonCharge_Required()
        {
            var engine = this.GetGenerator(mock => { });
            var app = this.GetFakeAppartement();
            app.ChargePerUnit = 2;
            app.CustomCharges.Clear();
            app.Dues = 0;

            Assert.True(engine.IsInvoiceRequired(app));
        }

        private ReceiptGenerator GetGenerator(Action<Mock<IExcelUtilities>> setupCallback)
        {
            setupCallback(mockGen);

            return new ReceiptGenerator(
                Options.Create(new InvoiceGeneratorSettings()),
                mockGen.Object,
                new Mock<IAmountToWords>().Object,
                new Mock<ILogger<ReceiptGenerator>>().Object);

        }

        private Appartement GetFakeAppartement()
        {
            return new Appartement(this.GetRandomString(), this.GetRandomString(), this.GetRandomString(), 1, 1);
        }

        private string GetRandomString()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
