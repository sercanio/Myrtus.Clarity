﻿using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.CMS.Domain.Users
{
    public static class UserErrors
    {
        public static readonly DomainError NotFound = new(
            "User.NotFound",
            404,
            "The user with the specified identifier was not found");

        public static readonly DomainError InvalidCredentials = new(
            "User.InvalidCredentials",
            401,
            "The provided credentials were invalid");

        public static readonly DomainError IdentityIdNotFound = new(
            "User.IdentityIdNotFound",
            500,
            "The identity id is not accessible");
    }
}
