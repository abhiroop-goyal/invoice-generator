using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace InvoiceGenerator.Tests
{
    public class DoubleToStringConverterTest
    {
        private readonly DoubleToStringConverter converter = new DoubleToStringConverter(
            new Mock<ILogger<DoubleToStringConverter>>().Object);

        [Fact]
        public void GetAmountInWords_Zero_CorrectResult()
        {
            Assert.Equal("Zero Rupee", this.converter.GetAmountInWords(0));
        }

        [Fact]
        public void GetAmountInWords_MultipleDecimalPlaces_CorrectResult()
        {
            Assert.Equal("Five Paise", this.converter.GetAmountInWords(0.05));
        }

        [Fact]
        public void GetAmountInWords_SingleDecimalPlace_CorrectResult()
        {
            Assert.Equal("Fifty Paise", this.converter.GetAmountInWords(0.5));
        }

        [Fact]
        public void GetAmountInWords_DecimalPlaceWithTrailingZero_CorrectResult()
        {
            Assert.Equal("Fifty Paise", this.converter.GetAmountInWords(0.50));
        }

        [Fact]
        public void GetAmountInWords_SingleDigit_CorrectResult()
        {
            Assert.Equal("Four Rupees", this.converter.GetAmountInWords(4));
        }

        [Fact]
        public void GetAmountInWords_SingleDigitWithTrailingZeroInDecimal_CorrectResult()
        {
            Assert.Equal("Four Rupees", this.converter.GetAmountInWords(4.0));
        }

        [Fact]
        public void GetAmountInWords_SingleDigitWithDecimal_CorrectResult()
        {
            Assert.Equal("Four Rupees Fifty Paise", this.converter.GetAmountInWords(4.5));
        }

        [Fact]
        public void GetAmountInWords_SpecialNumbers_CorrectResult()
        {
            Assert.Equal("Eleven Rupees", this.converter.GetAmountInWords(11));
        }

        [Fact]
        public void GetAmountInWords_MultipleDigitsWithTrailingZero_CorrectResult()
        {
            Assert.Equal("Thirty Rupees", this.converter.GetAmountInWords(30));
        }

        [Fact]
        public void GetAmountInWords_MultipleDigits_CorrectResult()
        {
            Assert.Equal("Sixty One Rupees", this.converter.GetAmountInWords(61));
        }

        [Fact]
        public void GetAmountInWords_SpecoalNumberHundred_CorrectResult()
        {
            Assert.Equal("One Hundred Rupees", this.converter.GetAmountInWords(100));
        }

        [Fact]
        public void GetAmountInWords_MultipleDigitsMoreThanHundred_CorrectResult()
        {
            Assert.Equal("One Hundred One Rupees", this.converter.GetAmountInWords(101));
        }

        [Fact]
        public void GetAmountInWords_MultipleDigitsMoreThanHundredWithSpecial_CorrectResult()
        {
            Assert.Equal("One Hundred Eleven Rupees", this.converter.GetAmountInWords(111));
        }

        [Fact]
        public void GetAmountInWords_MultipleDigitsMoreThanHundredSpecialNew_CorrectResult()
        {
            Assert.Equal("One Hundred Thirty Rupees", this.converter.GetAmountInWords(130));
        }

        [Fact]
        public void GetAmountInWords_SpecialThousand_CorrectResult()
        {
            Assert.Equal("One Thousand Rupees", this.converter.GetAmountInWords(1000));
        }

        [Fact]
        public void GetAmountInWords_SpecialThousandWithNumber_CorrectResult()
        {
            Assert.Equal("One Thousand Four Rupees", this.converter.GetAmountInWords(1004));
        }

        [Fact]
        public void GetAmountInWords_SpecialThousandWithSpecial_CorrectResult()
        {
            Assert.Equal("One Thousand Fifteen Rupees", this.converter.GetAmountInWords(1015));
        }

        [Fact]
        public void GetAmountInWords_SpecialThousandWithMultipleDigits_CorrectResult()
        {
            Assert.Equal("One Thousand Thirty Four Rupees", this.converter.GetAmountInWords(1034));
        }

        [Fact]
        public void GetAmountInWords_SpecialTenThousand_CorrectResult()
        {
            Assert.Equal("Ten Thousand Thirty Four Rupees", this.converter.GetAmountInWords(10034));
        }

        [Fact]
        public void GetAmountInWords_MoreThanTenThousand_CorrectResult()
        {
            Assert.Equal("Fourteen Thousand Thirty Four Rupees", this.converter.GetAmountInWords(14034));
        }

        [Fact]
        public void GetAmountInWords_NegativeNumbers_CorrectResult()
        {
            Assert.Equal("Negative Fourteen Thousand Thirty Four Rupees", this.converter.GetAmountInWords(-14034));
        }
    }
}