using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scribble.Content.Models;

namespace Scribble.Content.Infrastructure.Contexts.EntityConfigurations;

public class CommentEntityTypeConfiguration : IEntityTypeConfiguration<CommentEntity>
{
    public void Configure(EntityTypeBuilder<CommentEntity> builder)
    {
        builder.HasKey(e => e.Id).HasName("comments_pkey");

        builder.ToTable("comments");

        builder.Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasColumnName("comment_id");
        builder.Property(e => e.AuthorId).HasColumnName("author_id");
        builder.Property(e => e.Content).HasColumnName("content");
        builder.Property(e => e.PostId).HasColumnName("post_id");

        builder.HasOne(d => d.PostEntity).WithMany(p => p.Comments)
            .HasForeignKey(d => d.PostId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("comments_post_id_fkey");
    }
}