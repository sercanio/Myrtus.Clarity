﻿using Myrtus.CMS.Application.Permissions.Queries.GetAllPermissions;

namespace Myrtus.CMS.Application.Roles.Queries.GetAllRoles;

public sealed record GetAllRolesQueryResponse(Guid Id, string Name);