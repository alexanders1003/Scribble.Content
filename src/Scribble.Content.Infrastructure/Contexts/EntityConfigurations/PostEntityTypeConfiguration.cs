using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scribble.Content.Models;

namespace Scribble.Content.Infrastructure.Contexts.EntityConfigurations;

public class PostEntityTypeConfiguration : IEntityTypeConfiguration<PostEntity>
{
    public void Configure(EntityTypeBuilder<PostEntity> builder)
    {
        builder.HasKey(e => e.Id).HasName("posts_pkey");

        builder.ToTable("posts");

        builder.Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasColumnName("post_id");
        builder.Property(e => e.BlogId).HasColumnName("blog_id");
        builder.Property(e => e.Content).HasColumnName("content");
        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnType("timestamp without time zone")
            .HasColumnName("created_at");
        builder.Property(e => e.IsPosted).HasColumnName("is_posted");
        builder.Property(e => e.IsRejected).HasColumnName("is_rejected");
        builder.Property(e => e.PublishedAt)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("published_at");
        builder.Property(e => e.Title)
            .HasMaxLength(1000)
            .HasColumnName("title");
        builder.Property(e => e.UpdatedAt)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("updated_at");

        builder.HasOne(d => d.BlogEntity).WithMany(p => p.Posts)
            .HasForeignKey(d => d.BlogId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("posts_blog_id_fkey");

        builder.HasMany(d => d.Tags).WithMany(p => p.Posts)
            .UsingEntity<Dictionary<string, object>>(
                "PostsTag",
                r => r.HasOne<TagEntity>().WithMany()
                    .HasForeignKey("TagId")
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("posts_tags_tag_id_fkey"),
                l => l.HasOne<PostEntity>().WithMany()
                    .HasForeignKey("PostId")
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("posts_tags_post_id_fkey"),
                j =>
                {
                    j.HasKey("PostId", "TagId").HasName("posts_tags_pkey");
                    j.ToTable("posts_tags");
                    j.IndexerProperty<Guid>("PostId").HasColumnName("post_id");
                    j.IndexerProperty<Guid>("TagId").HasColumnName("tag_id");
                });
    }
}