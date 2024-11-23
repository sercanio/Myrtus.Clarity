﻿using Myrtus.CMS.Application.Features.Permissions.Queries.GetAllPermissions;
using System.Collections.ObjectModel;

namespace Myrtus.CMS.Application.Features.Roles.Queries.GetRoleById
{
    public sealed record GetRoleByIdQueryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
        public ICollection<GetAllPermissionsQueryResponse> Permissions { get; set; } = [];

        public GetRoleByIdQueryResponse(Guid id, string name, bool isDefault)
        {
            Id = id;
            Name = name;
            IsDefault = isDefault;
        }

        public GetRoleByIdQueryResponse(
            Guid id,
            string name,
            bool isDeault,
            Collection<GetAllPermissionsQueryResponse> permissions)
        {
            Id = id;
            Name = name;
            IsDefault = isDeault;
            Permissions = permissions;
        }

        public GetRoleByIdQueryResponse() { }
    };
}