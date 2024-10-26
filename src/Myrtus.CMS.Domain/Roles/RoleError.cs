﻿using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.CMS.Domain.Roles;

public static class RoleErrors
{
    public static readonly Error NotFound = new(
        "Role.NotFound",
        404,
        "The role with the specified identifier was not found");

    public static readonly Error Overlap = new(
        "Role.Overlap",
        409,
        "The current role is overlapping with an existing one");
}
