namespace InvoiceGenerator
{
    public class DoubleToStringConverter : BaseClass, IAmountToWords
    {
        /// <summary>
        /// Dictionary for single digit.
        /// </summary>
        private readonly Dictionary<int, string> onesDigit;

        /// <summary>
        /// Dictionar for tens digit.
        /// </summary>
        private readonly Dictionary<int, string> tensDigit;

        /// <summary>
        /// Dictionar for special tens digit.
        /// </summary>
        private readonly Dictionary<int, string> specialTensDigit;

        /// <summary>
        /// Dictionar for special tens digit.
        /// </summary>
        private readonly Dictionary<int, string> noOfZerosToString;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleToStringConverter"/> class.
        /// </summary>
        /// <param name="_logger">Logger class.</param>
        public DoubleToStringConverter(ILogger _logger) : base(_logger)
        {
            this.onesDigit = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" },
                { 3, "Three" },
                { 4, "Four" },
                { 5, "Five" },
                { 6, "Six" },
                { 7, "Seven" },
                { 8, "Eight" },
                { 9, "Nine" }
            };

            this.specialTensDigit = new Dictionary<int, string>
            {
                { 11, "Eleven" },
                { 12, "Twelve" },
                { 13, "Thirteen" },
                { 14, "Fourteen" },
                { 15, "Fifteen" },
                { 16, "Sixteen" },
                { 17, "Seventeen" },
                { 18, "Eighteen" },
                { 19, "Nineteen" }
            };

            this.tensDigit = new Dictionary<int, string>
            {
                { 10, "Ten" },
                { 20, "Twenty" },
                { 30, "Thirty" },
                { 40, "Fourty" },
                { 50, "Fifty" },
                { 60, "Sixty" },
                { 70, "Seventy" },
                { 80, "Eighty" },
                { 90, "Ninety" }
            };

            this.noOfZerosToString = new Dictionary<int, string>
            {
                { 2, "Hundred" },
                { 3, "Thousand" },
                { 5, "Lakh" },
                { 7, "Crore" },
                { 9, "Arab" }
            };
        }

        /// <inheritdoc/>
        public string GetAmountInWords(double amount)
        {
            bool isNegative = amount < 0;

            List<string> parts = new List<string>();
            if (isNegative)
            {
                parts.Add("Negative");
                amount = isNegative ? amount * -1 : amount;
            }

            int rupees = ((int)amount);
            int paise = (int)((amount - rupees) * 100);
            if (rupees != 0)
            {
                parts.Add($"{this.GetStringRepresentation(rupees)} Rupees");
            }

            if (paise != 0)
            {
                parts.Add($"{this.GetStringRepresentation(paise)} Paise");
            }

            if (parts.Count == 0)
            {
                parts.Add("Zero Rupees");
            }

            return string.Join(' ', parts);
        }

        /// <summary>
        /// Get string representation of number.
        /// </summary>
        /// <param name="amount">Amount as an integer.</param>
        /// <returns>The string representation.</returns>
        private string GetStringRepresentation(int amount)
        {
            if (amount == 0)
            {
                return String.Empty;
            }

            if (amount < 10)
            {
                return this.GetOnesDigit(amount);
            }
            else if (amount < 100)
            {
                return this.GetTensDigit(amount);
            }
            else if (amount < 1000)
            {
                List<string> parts = new List<string>
                {
                    this.GetOnesDigit(amount / 100),
                    "Hundred",
                    this.GetStringRepresentation(amount % 100)
                };

                return JoinStrings(parts);
            }
            else
            {
                IEnumerable<string> parts = this.ProcessLargeNumber(amount);
                return JoinStrings(parts);
            }
        }

        /// <summary>
        /// Process large number.
        /// </summary>
        /// <param name="amount">Amount as integer.</param>
        /// <returns></returns>
        private IEnumerable<string> ProcessLargeNumber(int amount)
        {
            int length = amount.ToString().Length;
            int key = ((length / 2) * 2) - 1;
            int powerOf10 = (int)Math.Pow(10, key);
            string suffix = this.noOfZerosToString[key];

            List<string> parts = new List<string>();
            int digitsToProcess = amount / powerOf10;
            int remainder = amount % powerOf10;
            if (digitsToProcess / 10 == 0)
            {
                parts.Add(this.GetOnesDigit(digitsToProcess));
            }
            else
            {
                parts.Add(this.GetTensDigit(digitsToProcess));
            }

            parts.Add(suffix);
            parts.Add(this.GetStringRepresentation(remainder));
            return parts;
        }

        /// <summary>
        /// Get ones digit.
        /// </summary>
        /// <param name="amount">Amount as integer.</param>
        /// <returns>String representation.</returns>
        private string GetOnesDigit(int amount)
        {
            if (this.onesDigit.ContainsKey(amount))
            {
                return this.onesDigit[amount];
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Get tens digit.
        /// </summary>
        /// <param name="amount">Amount as integer.</param>
        /// <returns>String representation.</returns>
        private string GetTensDigit(int amount)
        {
            if (this.specialTensDigit.ContainsKey(amount))
            {
                return this.specialTensDigit[amount];
            }
            else
            {
                List<string> items = new List<string>();
                int tensDigit = (amount / 10) * 10;
                items.Add(this.tensDigit[tensDigit]);
                items.Add(this.GetOnesDigit(amount % 10));
                return JoinStrings(items);
            }
        }

        /// <summary>
        /// Join multipe strings.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private static string JoinStrings(IEnumerable<string> items)
        {
            return string.Join(' ', items.Where(x => !string.IsNullOrWhiteSpace(x)));
        }
    }
}
