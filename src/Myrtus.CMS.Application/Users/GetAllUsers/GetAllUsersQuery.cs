﻿using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.CMS.Application.Users.GetUser;

namespace Myrtus.CMS.Application.Users.GetAllUsers;

public sealed record GetAllUsersQuery(
    int PageIndex, 
    int PageSize) : IQuery<IPaginatedList<GetUserQueryResponse>>;