using Myrtus.CMS.Domain.Users;

namespace Myrtus.CMS.Domain.Roles
{
    public sealed class RoleUser
    {
        public Guid RoleId { get; set; }
        public Guid UserId { get; set; }

        public required Role Role { get; set; }
        public required User User { get; set; }
    }
}
