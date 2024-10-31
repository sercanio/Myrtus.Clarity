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

        builder.Property(blog => blog.Title)
            .IsRequired()
            .HasMaxLength(255)
            .HasConversion(
                title => title.Value,
                value => new Title(value)
            );

        builder.Property(blog => blog.Slug)
            .IsRequired()
            .HasConversion(
                slug => slug.Value,
                value => new Slug(value)
            );

        builder.HasMany(blog => blog.Posts)
            .WithOne(post => post.Blog)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(blog => blog.LastUpdatedBy)
            .WithMany()
            .HasForeignKey("LastUpdatedById")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(blog => blog.DeletedBy)
            .WithMany()
            .HasForeignKey("DeletedById")
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(blog => blog.UpdateReason)
            .HasMaxLength(500)
            .HasConversion(
                reason => reason != null ? reason.Value : null,
                value => value != null ? new Description(value) : null
            );

        builder.Property(blog => blog.DeleteReason)
            .HasMaxLength(500)
            .HasConversion(
                reason => reason != null ? reason.Value : null,
                value => value != null ? new Description(value) : null
            );
    }
}
