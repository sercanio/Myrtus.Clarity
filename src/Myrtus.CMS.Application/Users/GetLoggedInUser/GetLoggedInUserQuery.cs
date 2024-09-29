using Myrtus.Clarity.Core.Application.Abstractions.Messaging;

namespace Myrtus.CMS.Application.Users.GetLoggedInUser;

public sealed record GetLoggedInUserQuery : IQuery<UserResponse>;
