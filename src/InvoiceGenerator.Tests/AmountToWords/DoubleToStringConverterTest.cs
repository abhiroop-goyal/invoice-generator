using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace InvoiceGenerator.Tests
{
    [TestClass]
    public class DoubleToStringConverterTest
    {
        [TestMethod]
        public void AmountToWordsTest()
        {
            DoubleToStringConverter converter = new DoubleToStringConverter(
                new Mock<ILogger<DoubleToStringConverter>>().Object);

            Assert.IsTrue(converter.GetAmountInWords(0) == "Zero Rupees");
            Assert.IsTrue(converter.GetAmountInWords(0.05) == "Five Paise");
            Assert.IsTrue(converter.GetAmountInWords(0.5) == "Fifty Paise");
            Assert.IsTrue(converter.GetAmountInWords(0.50) == "Fifty Paise");
            Assert.IsTrue(converter.GetAmountInWords(4) == "Four Rupees");
            Assert.IsTrue(converter.GetAmountInWords(4.0) == "Four Rupees");
            Assert.IsTrue(converter.GetAmountInWords(4.5) == "Four Rupees Fifty Paise");
            Assert.IsTrue(converter.GetAmountInWords(11) == "Eleven Rupees");
            Assert.IsTrue(converter.GetAmountInWords(30) == "Thirty Rupees");
            Assert.IsTrue(converter.GetAmountInWords(61) == "Sixty One Rupees");
            Assert.IsTrue(converter.GetAmountInWords(100) == "One Hundred Rupees");
            Assert.IsTrue(converter.GetAmountInWords(101) == "One Hundred One Rupees");
            Assert.IsTrue(converter.GetAmountInWords(111) == "One Hundred Eleven Rupees");
            Assert.IsTrue(converter.GetAmountInWords(130) == "One Hundred Thirty Rupees");
            Assert.IsTrue(converter.GetAmountInWords(1000) == "One Thousand Rupees");
            Assert.IsTrue(converter.GetAmountInWords(1004) == "One Thousand Four Rupees");
            Assert.IsTrue(converter.GetAmountInWords(1015) == "One Thousand Fifteen Rupees");
            Assert.IsTrue(converter.GetAmountInWords(1034) == "One Thousand Thirty Four Rupees");
            Assert.IsTrue(converter.GetAmountInWords(10034) == "Ten Thousand Thirty Four Rupees");
            Assert.IsTrue(converter.GetAmountInWords(14034) == "Fourteen Thousand Thirty Four Rupees");
            Assert.IsTrue(converter.GetAmountInWords(-14034) == "Negative Fourteen Thousand Thirty Four Rupees");
        }
    }
}