﻿using Myrtus.CMS.Application.Roles.Queries.GetRoleById;
using Myrtus.CMS.Application.Users.GetLoggedInUser;

namespace Myrtus.CMS.Application.Users.Queries.GetLoggedInUser;

public sealed record UserResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<LoggedInUserRolesDto> Roles { get; set; }    

    public UserResponse(Guid id, string email, string firstName, string lastName, List<LoggedInUserRolesDto> roles)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Roles = roles;
    }

    internal UserResponse() { }
}
