using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scribble.Content.Models;

namespace Scribble.Content.Infrastructure.Contexts.EntityConfigurations;

public class LikeEntityTypeConfiguration : IEntityTypeConfiguration<LikeEntity>
{
    public void Configure(EntityTypeBuilder<LikeEntity> builder)
    {
        builder.HasKey(e => e.Id).HasName("likes_pkey");

        builder.ToTable("likes");

        builder.Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasColumnName("like_id");
        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnType("timestamp without time zone")
            .HasColumnName("created_at");
        builder.Property(e => e.PostId).HasColumnName("post_id");
        builder.Property(e => e.UserId).HasColumnName("user_id");

        builder.HasOne(d => d.PostEntity).WithMany(p => p.Likes)
            .HasForeignKey(d => d.PostId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("likes_post_id_fkey");
    }
}