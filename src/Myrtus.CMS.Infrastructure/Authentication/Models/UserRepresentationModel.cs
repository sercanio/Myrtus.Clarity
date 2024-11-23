﻿using Myrtus.Clarity.Core.Infrastructure.Authentication.Keycloak.Models;
using Myrtus.CMS.Domain.Users;

namespace Myrtus.CMS.Infrastructure.Authentication.Models
{
    internal sealed class UserRepresentationModel
    {
        public Dictionary<string, string> Access { get; set; } = [];

        public Dictionary<string, List<string>> Attributes { get; set; } = [];

        public Dictionary<string, string> ClientRoles { get; set; } = [];

        public long? CreatedTimestamp { get; set; }

        public CredentialRepresentationModel[] Credentials { get; set; } = [];

        public string[] DisableableCredentialTypes { get; set; } = [];

        public string Email { get; set; } = string.Empty;

        public bool? EmailVerified { get; set; }

        public bool? Enabled { get; set; }

        public string FederationLink { get; set; } = string.Empty;

        public string Id { get; set; } = string.Empty;

        public string[] Groups { get; set; } = [];

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

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
                Username = user.Email,
                Enabled = true,
                EmailVerified = true,
                CreatedTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Attributes = [],
                RequiredActions = []
            };
    }
}
