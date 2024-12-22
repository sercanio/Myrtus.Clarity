using Myrtus.Clarity.Core.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myrtus.Clarity.Domain.Users.ValueObjects
{
    public class FirstName : ValueObject
    {
        public string Value { get; }

        public FirstName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("First name cannot be empty", nameof(value));
            }

            if (value.Length > 50)
            {
                throw new ArgumentException("First name cannot be longer than 50 characters", nameof(value));
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
