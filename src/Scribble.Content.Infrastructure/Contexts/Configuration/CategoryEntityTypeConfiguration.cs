using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scribble.Content.Models;

namespace Scribble.Content.Infrastructure.Contexts.Configuration;

public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<CategoryEntity>
{
    public void Configure(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder.ToTable("categories").HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("category_id")
            .HasDefaultValueSql("NEWID()");

        builder.Property(x => x.Name).HasColumnName("category_name")
            .IsRequired()
            .HasMaxLength(100);
    }
}