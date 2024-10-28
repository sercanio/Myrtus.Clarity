using Myrtus.CMS.Domain.Users;

namespace Myrtus.CMS.Domain.Roles;

public sealed class RoleUser
{
    public Guid RoleId { get; set; }
    public Guid UserId { get; set; }

    public Role Role { get; set; }
    public User User { get; set; }

}
