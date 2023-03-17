using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scribble.Content.Models;

namespace Scribble.Content.Infrastructure.Contexts.Configuration;

public class ArticleEntityTypeConfiguration : IEntityTypeConfiguration<ArticleEntity>
{
    public void Configure(EntityTypeBuilder<ArticleEntity> builder)
    {
        builder.ToTable("articles").HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("article_id")
            .HasDefaultValueSql("NEWID()");

        builder.Property(x => x.Title).HasColumnName("title")
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Content).HasColumnName("content");

        builder.Property(x => x.CreatedAt).HasColumnName("created_at")
            .HasDefaultValueSql("GETDATE()");

        builder.Property(x => x.PublishedAt).HasColumnName("published_at");

        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");

        builder.Property(x => x.IsPublished).HasColumnName("is_published");
    }
}