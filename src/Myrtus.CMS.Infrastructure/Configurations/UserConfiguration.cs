using Myrtus.CMS.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Myrtus.CMS.Infrastructure.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public static Guid AdminId { get; private set; }

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.FirstName)
            .HasMaxLength(200)
            .HasConversion(firstName => firstName.Value, value => new FirstName(value));

        builder.Property(user => user.LastName)
            .HasMaxLength(200)
            .HasConversion(lastName => lastName.Value, value => new LastName(value));

        builder.Property(user => user.Email)
            .HasMaxLength(400)
            .HasConversion(email => email.Value, value => new Email(value));

        builder.HasIndex(user => user.Email).IsUnique();
        builder.HasIndex(user => user.IdentityId).IsUnique();

        // Seed Admin user without roles
        var adminUser = User.CreateWithoutRolesForSeeding(
            new FirstName("Admin"),
            new LastName("Admin"),
            new Email("admin@email.com"));
        adminUser.SetIdentityId("a67c921a-d8b5-4e1e-a741-ee021f6ba29f");

        AdminId = adminUser.Id;

        builder.HasData(adminUser);
    }
}

