using Myrtus.Clarity.Core.Infrastructure.Authentication.Azure.Models;
using Myrtus.Clarity.Domain.Users;
using Myrtus.Clarity.Domain.Users.ValueObjects;

namespace Myrtus.Clarity.Infrastructure.Authentication.Models
{
    internal sealed class UserRepresentationModel
    {
        public Dictionary<string, string> Access { get; set; } = [];

        public Dictionary<string, List<string>> Attributes { get; set; } = [];

        public Dictionary<string, string> ClientRoles { get; set; } = [];

        public long? CreatedTimestamp { get; set; }

        public CredentialRepresentationModel[] Credentials { get; set; } = [];

        public string[] DisableableCredentialTypes { get; set; } = [];

        public Email Email { get; set; } = new Email(string.Empty);

        public bool? EmailVerified { get; set; }

        public bool? Enabled { get; set; }

        public string FederationLink { get; set; } = string.Empty;

        public string Id { get; set; } = string.Empty;

        public string[] Groups { get; set; } = [];

        public FirstName FirstName { get; set; } = new FirstName(string.Empty);

        public LastName LastName { get; set; } = new LastName(string.Empty);

        public int? NotBefore { get; set; }

        public string Origin { get; set; } = string.Empty;

        public string[] RealmRoles { get; set; } = [];

        public string[] RequiredActions { get; set; } = [];

        public string Self { get; set; } = string.Empty;

        public string ServiceAccountClientId { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        internal static UserRepresentationModel FromUser(User user) =>
            new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Email.Value,
                Enabled = true,
                EmailVerified = true,
                CreatedTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Attributes = [],
                RequiredActions = []
            };
    }
}
