using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.Clarity.Domain.Users.ValueObjects
{
    public class LastName : ValueObject
    {
        public string Value { get; }

        public LastName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Last name cannot be empty", nameof(value));
            }

            if (value.Length > 50)
            {
                throw new ArgumentException("Last name cannot be longer than 50 characters", nameof(value));
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
