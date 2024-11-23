namespace Myrtus.Clarity.Domain.Users.Events
{
    public sealed record UserDomainEvents
    {
        public static readonly string Created = "UserCreated";
        public static readonly string Deleted = "UserDeleted";
        public static readonly string UpdatedName = "UserNameUpdated";
        public static readonly string UpdatedEmail = "UserEmailUpdated";
        public static readonly string UpdatedPassword = "UserPasswordUpdated";
        public static readonly string AddedRole = "UserRoleAdded";
        public static readonly string RemovedRole = "UserRoleRemoved";
        public static readonly string UserLoggedIn = "UserLoggedIn";
    }
}
