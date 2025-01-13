using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Myrtus.Clarity.Domain.Roles;
using Myrtus.Clarity.Domain.Users;
using Myrtus.Clarity.Domain.Users.ValueObjects;

namespace Myrtus.Clarity.Infrastructure.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.HasKey(user => user.Id);

            // Column Mappings
            builder.Property(u => u.IdentityId)
                   .HasColumnName("identity_id")
                   .IsRequired();

            // Indices
            builder.HasIndex(u => u.IdentityId).IsUnique();

            // Value Objects (Owned Types)
            builder.OwnsOne(u => u.FirstName, firstName =>
            {
                firstName.Property(f => f.Value).HasColumnName("first_name");
            });

            builder.OwnsOne(u => u.LastName, lastName =>
            {
                lastName.Property(l => l.Value).HasColumnName("last_name");
            });

            builder.OwnsOne(u => u.Email, email =>
            {
                email.Property(e => e.Value).HasColumnName("email");
                email.HasIndex(e => e.Value).IsUnique();
            });

            builder.OwnsOne(u => u.NotificationPreference, np =>
            {
                np.Property(p => p.IsInAppNotificationEnabled).HasColumnName("in_app_notification");
                np.Property(p => p.IsEmailNotificationEnabled).HasColumnName("email_notification");
                np.Property(p => p.IsPushNotificationEnabled).HasColumnName("push_notification");
            });

            var adminUser = User.CreateWithoutRolesForSeeding(
                new FirstName("Sercan"),
                new LastName("Ateş"),
                new Email("sercanates91@gmail.com")
            );
            adminUser.SetIdentityId("b3398ff2-1b43-4af7-812d-eb4347eecbb8");

            // Basic entity seeding
            builder.HasData(
                new
                {
                    adminUser.Id,
                    IdentityId = adminUser.IdentityId,
                    CreatedBy = adminUser.CreatedBy,
                    CreatedOnUtc = adminUser.CreatedOnUtc,
                    // EF will set the rest of the base fields with default values if needed
                }
            );

            // Owned type seeding
            builder.OwnsOne(u => u.FirstName).HasData(
                new { UserId = adminUser.Id, Value = adminUser.FirstName.Value }
            );
            builder.OwnsOne(u => u.LastName).HasData(
                new { UserId = adminUser.Id, Value = adminUser.LastName.Value }
            );
            builder.OwnsOne(u => u.Email).HasData(
                new { UserId = adminUser.Id, Value = adminUser.Email.Value }
            );
            builder.OwnsOne(u => u.NotificationPreference).HasData(
                new
                {
                    UserId = adminUser.Id,
                    IsInAppNotificationEnabled = adminUser.NotificationPreference.IsInAppNotificationEnabled,
                    IsEmailNotificationEnabled = adminUser.NotificationPreference.IsEmailNotificationEnabled,
                    IsPushNotificationEnabled = adminUser.NotificationPreference.IsPushNotificationEnabled
                }
            );
        }
    }
}
