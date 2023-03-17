using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scribble.Content.Models;

namespace Scribble.Content.Infrastructure.Contexts.Configuration;

public class BlogEntityTypeConfiguration : IEntityTypeConfiguration<BlogEntity>
{
    public void Configure(EntityTypeBuilder<BlogEntity> builder)
    {
        builder.ToTable("blogs").HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("blog_id")
            .HasDefaultValueSql("NEWID()");

        builder.Property(x => x.Name).HasColumnName("blog_name").IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description).HasColumnName("description")
            .HasMaxLength(1000);
        
        builder.Property(x => x.CreatedAt).HasColumnName("created_at")
            .HasDefaultValueSql("GETDATE()");

        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");

        builder.Property(x => x.AuthorId).HasColumnName("author_id")
            .IsRequired();
    }
}