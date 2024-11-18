using Myrtus.Clarity.Core.Application.Abstractions.Messaging;

namespace Myrtus.CMS.Application.Features.Users.Queries.GetLoggedInUser;

public sealed record GetLoggedInUserQuery : IQuery<UserResponse>;
