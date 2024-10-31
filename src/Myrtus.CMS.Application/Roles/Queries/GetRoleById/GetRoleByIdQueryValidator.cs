using FluentValidation;

namespace Myrtus.CMS.Application.Roles.Queries.GetRoleById;

public class GetRoleByIdQueryValidator : AbstractValidator<GetRoleByIdQuery>
{
    public GetRoleByIdQueryValidator()
    {
        RuleFor(r => r.RoleId).NotEmpty();
    }
}