namespace Myrtus.Clarity.Core.Application.Abstractions.Authentication.Keycloak;

public interface IUserContext
{
    Guid UserId { get; }

    string IdentityId { get; }
}
