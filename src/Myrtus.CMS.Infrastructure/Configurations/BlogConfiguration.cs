using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Myrtus.CMS.Domain.Blogs;
using Myrtus.CMS.Domain.Blogs.Common;
using Myrtus.CMS.Domain.Common;

namespace Myrtus.Infrastructure.Configurations;

internal sealed class BlogConfiguration : IEntityTypeConfiguration<Blog>
{
    public void Configure(EntityTypeBuilder<Blog> builder)
    {
        builder.ToTable("blogs");

        builder.HasKey(blog => blog.Id);
        builder.HasIndex(blog => blog.Id);

        // Configuring Title as required with a max length and value conversion
        builder.Property(blog => blog.Title)
            .IsRequired()
            .HasMaxLength(255)
            .HasConversion(
                title => title.Value,
                value => new Title(value)
            );

        // Configuring Slug as required with value conversion
        builder.Property(blog => blog.Slug)
            .IsRequired()
            .HasConversion(
                slug => slug.Value,
                value => new Slug(value)
            );

        // Configure the relationship for Posts
        builder.HasMany(blog => blog.Posts)
            .WithOne(post => post.Blog)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure LastUpdatedBy relationship (optional user reference)
        builder.HasOne(blog => blog.LastUpdatedBy)
            .WithMany() // No inverse navigation on User
            .HasForeignKey("LastUpdatedById")
            .OnDelete(DeleteBehavior.Restrict);

        // Configure DeletedBy relationship (optional user reference)
        builder.HasOne(blog => blog.DeletedBy)
            .WithMany() // No inverse navigation on User
            .HasForeignKey("DeletedById")
            .OnDelete(DeleteBehavior.Restrict);

        // Configuring UpdateReason as a value object or string
        builder.Property(blog => blog.UpdateReason)
            .HasMaxLength(500) // Assuming it's a description, you can adjust the length
            .HasConversion(
                reason => reason != null ? reason.Value : null, // Null check for nullable value
                value => value != null ? new Description(value) : null
            );

        // Configuring DeleteReason as a value object or string
        builder.Property(blog => blog.DeleteReason)
            .HasMaxLength(500) // Assuming it's a description, you can adjust the length
            .HasConversion(
                reason => reason != null ? reason.Value : null, // Null check for nullable value
                value => value != null ? new Description(value) : null
            );
    }
}
