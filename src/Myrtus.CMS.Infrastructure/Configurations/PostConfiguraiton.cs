using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Myrtus.CMS.Domain.Blogs.Common;
using Myrtus.CMS.Domain.Blogs.Posts;

namespace Myrtus.Infrastructure.Configurations;

internal sealed class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("posts");

        builder.HasKey(post => post.Id);

        builder.Property(post => post.Title)
            .IsRequired()
            .HasMaxLength(255)
            .HasConversion(
                title => title.Value,
                value => new Title(value)
            );

        builder.Property(post => post.Slug)
            .IsRequired()
            .HasConversion(
                slug => slug.Value,
                value => new Slug(value)
            );

        builder.Property(post => post.Summary)
            .IsRequired()
            .HasMaxLength(500)
            .HasConversion(
                summary => summary.Value,
                value => new Summary(value)
            );

        builder.Property(post => post.Content)
            .IsRequired()
            .HasConversion(
                content => content.Value,
                value => new Content(value)
            );

        builder.Property(post => post.CoverImage)
            .HasConversion(
                coverImageUrl => coverImageUrl.Value,
                value => new CoverImageUrl(value)
            );

        builder.Property(post => post.CardImage)
            .HasConversion(
                cardImageUrl => cardImageUrl.Value,
                value => new CardImageUrl(value)
            );

        builder.Property(post => post.Status)
            .HasConversion(
                status => status.Value,
                value => PostStatus.FromValue(value)
            );  
        
        builder.Property(post => post.Reviewed)
            .HasConversion(
                reviewed => reviewed.Value,
                value => new Reviewed(value)
            );

        builder.HasOne(post => post.Blog)
            .WithMany(blog => blog.Posts)
            .HasForeignKey("BlogId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}