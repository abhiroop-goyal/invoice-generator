namespace InvoiceGenerator.Tests
{
    using System;

    /// <summary>
    /// Sample contract for parser test.
    /// </summary>
    class ParserTestContract : IComparable
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Age.
        /// </summary>
        public double Age { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is ParserTestContract contract &&
                   string.Equals(Name, contract.Name) &&
                   Age == contract.Age;
        }

        /// <inheritdoc/>
        public int CompareTo(object? obj)
        {
            if (obj is ParserTestContract)
            {
                return -1;
            }

            return this.Age.CompareTo((obj as ParserTestContract).Age);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.Age.GetHashCode() + this.Name.GetHashCode();
        }
    }
}
