namespace Farsica.Framework.Data.Enumeration
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public abstract class Enumeration : IComparable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Enumeration"/> class.
        /// Creates instance of type <see cref="Enumeration" />.
        /// </summary>
        /// <remarks>
        /// This constructor should not be called from the derived class.
        /// It is helpful in doing JSON Serialization or mapping through Automapper.
        /// </remarks>
        [ExcludeFromCodeCoverage]
        protected Enumeration()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Enumeration"/> class.
        /// Creates instance of type <see cref="Enumeration" />.
        /// </summary>
        /// <param name="value">The Enumeration value.</param>
        /// <param name="name">The Enumeration name.</param>
        protected Enumeration(string name, int value)
        {
            Value = value;
            Name = name;
        }

        /// <summary>
        /// Gets the <see cref="string"/> Enumeration name.
        /// </summary>
        public string? Name { get; }

        /// <summary>
        /// Gets the <see cref="int"/> Enumeration value.
        /// </summary>
        public int Value { get; }

        /// <inheritdoc/>
        public override string? ToString() => Name;

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj is not Enumeration otherValue)
            {
                return false;
            }

            var typeMatches = GetType() == obj.GetType();
            var valueMatches = Value.Equals(otherValue.Value);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode() => Value.GetHashCode();

        public int CompareTo(object? other)
        {
            return Value.CompareTo((other as Enumeration)?.Value);
        }
    }
}
