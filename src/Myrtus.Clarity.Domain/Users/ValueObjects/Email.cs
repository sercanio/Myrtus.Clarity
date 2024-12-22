using Myrtus.Clarity.Core.Domain.Abstractions;
using System.Text.RegularExpressions;

namespace Myrtus.Clarity.Domain.Users.ValueObjects
{
    public class Email : ValueObject
    {
        private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        public string Value { get; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Email cannot be empty", nameof(value));
            }

            if (!EmailRegex.IsMatch(value))
            {
                throw new ArgumentException("Invalid email format", nameof(value));
            }

            Value = value;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
