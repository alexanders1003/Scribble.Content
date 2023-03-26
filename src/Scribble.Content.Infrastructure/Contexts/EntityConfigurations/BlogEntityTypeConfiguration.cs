using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scribble.Content.Models;

namespace Scribble.Content.Infrastructure.Contexts.EntityConfigurations;

public class BlogEntityTypeConfiguration : IEntityTypeConfiguration<BlogEntity>
{
    public void Configure(EntityTypeBuilder<BlogEntity> builder)
    {
        builder.HasKey(e => e.Id).HasName("blogs_pkey");

        builder.ToTable("blogs");

        builder.Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasColumnName("blog_id");
        builder.Property(e => e.AuthorId).HasColumnName("author_id");
        builder.Property(e => e.BlogName)
            .HasMaxLength(200)
            .HasColumnName("blog_name");
        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnType("timestamp without time zone")
            .HasColumnName("created_at");
        builder.Property(e => e.Description)
            .HasMaxLength(1000)
            .HasColumnName("description");
        builder.Property(e => e.RemovedAt)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("removed_at");
        builder.Property(e => e.UpdatedAt)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("updated_at");

        builder.HasMany(d => d.Categories).WithMany(p => p.Blogs)
            .UsingEntity<Dictionary<string, object>>(
                "BlogsCategory",
                r => r.HasOne<CategoryEntity>().WithMany()
                    .HasForeignKey("CategoryId")
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("blogs_categories_category_id_fkey"),
                l => l.HasOne<BlogEntity>().WithMany()
                    .HasForeignKey("BlogId")
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("blogs_categories_blog_id_fkey"),
                j =>
                {
                    j.HasKey("BlogId", "CategoryId").HasName("blogs_categories_pkey");
                    j.ToTable("blogs_categories");
                    j.IndexerProperty<Guid>("BlogId").HasColumnName("blog_id");
                    j.IndexerProperty<Guid>("CategoryId").HasColumnName("category_id");
                });
    }
}