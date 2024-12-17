using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Myrtus.Clarity.Domain.Users;

namespace Myrtus.Clarity.Infrastructure.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public static Guid AdminId { get; }

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.HasKey(user => user.Id);
            builder.HasIndex(user => user.Email).IsUnique();
            builder.HasIndex(user => user.IdentityId).IsUnique();

            builder.OwnsOne(user => user.NotificationPreference, np =>
            {
                np.Property(p => p.IsInAppNotificationEnabled).HasColumnName("in_app_notification");
                np.Property(p => p.IsEmailNotificationEnabled).HasColumnName("email_notification");
                np.Property(p => p.IsPushNotificationEnabled).HasColumnName("push_notification");
            });
        }
    }
}
