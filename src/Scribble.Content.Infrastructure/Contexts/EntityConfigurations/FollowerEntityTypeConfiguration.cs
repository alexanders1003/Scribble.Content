using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scribble.Content.Models;

namespace Scribble.Content.Infrastructure.Contexts.EntityConfigurations;

public class FollowerEntityTypeConfiguration : IEntityTypeConfiguration<FollowerEntity>
{
    public void Configure(EntityTypeBuilder<FollowerEntity> builder)
    {
        builder.HasKey(e => e.Id).HasName("followers_pkey");

        builder.ToTable("followers");

        builder.Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasColumnName("id");
        builder.Property(e => e.BlogId).HasColumnName("blog_id");
        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnType("timestamp without time zone")
            .HasColumnName("created_at");
        builder.Property(e => e.FollowerId).HasColumnName("follower_id");

        builder.HasOne(d => d.BlogEntity).WithMany(p => p.Followers)
            .HasForeignKey(d => d.BlogId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("followers_blog_id_fkey");
    }
}