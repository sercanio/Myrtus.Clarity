using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Myrtus.CMS.Domain.Blogs.Posts;

namespace Myrtus.Infrastructure.Configurations;

internal sealed class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("comments");

        builder.HasKey(comment => comment.Id);
        builder.HasIndex(comment => comment.Id);

        builder.Property(comment => comment.Content)
            .IsRequired()
            .HasConversion(
                content => content.Value,
                value => new Content(value)
            );

        builder.HasOne(comment => comment.Post)
            .WithMany()
            .HasForeignKey("PostId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
