﻿using Myrtus.CMS.Domain.Users;

namespace Myrtus.CMS.WebAPI.Controllers;

internal static class Permissions
{
    public const string UsersRead = "users:read";
    public const string UsersCreate = "users:create";
    public const string UsersUpdate= "users:update";
    public const string UsersDelete= "users:delete";
    
    public const string RolesRead = "roles:read";
    public const string RolesCreate = "roles:create";
    public const string RolesUpdate = "roles:update";
    public const string RolesDelete = "roles:delete";

    public const string PermissionsRead = "permissions:read";
}
