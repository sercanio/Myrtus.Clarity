using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Myrtus.Clarity.Domain.Users;

namespace Myrtus.Clarity.Infrastructure.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.HasKey(user => user.Id);

            builder.OwnsOne(user => user.FirstName, firstName =>
            {
                firstName.Property(f => f.Value).HasColumnName("first_name");
            });

            builder.OwnsOne(user => user.LastName, lastName =>
            {
                lastName.Property(l => l.Value).HasColumnName("last_name");
            });

            builder.OwnsOne(user => user.Email, email =>
            {
                email.Property(e => e.Value).HasColumnName("email");
                email.HasIndex(e => e.Value).IsUnique();
            });

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
