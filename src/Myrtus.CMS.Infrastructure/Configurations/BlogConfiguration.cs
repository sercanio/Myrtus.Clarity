using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Myrtus.CMS.Domain.Blogs;
using Myrtus.CMS.Domain.Blogs.Common;

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
    }
}
