using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scribble.Content.Models;

namespace Scribble.Content.Infrastructure.Contexts.Configuration;

public class CommentEntityTypeConfiguration : IEntityTypeConfiguration<CommentEntity>
{
    public void Configure(EntityTypeBuilder<CommentEntity> builder)
    {
        builder.ToTable("comments").HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("comment_id")
            .HasDefaultValueSql("NEWID()");

        builder.Property(x => x.Text).HasColumnName("text")
            .IsRequired();

        builder.Property(x => x.CreatedAt).HasColumnName("created_at")
            .HasDefaultValueSql("GETDATE()");

        builder.Property(x => x.PublishedAt).HasColumnName("published_at");

        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");

        builder.Property(x => x.AuthorId).HasColumnName("author_id")
            .IsRequired();
    }
}