using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scribble.Content.Models;

namespace Scribble.Content.Infrastructure.Contexts.EntityConfigurations;

public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<CategoryEntity>
{
    public void Configure(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder.HasKey(e => e.Id).HasName("categories_pkey");

        builder.ToTable("categories");

        builder.HasIndex(e => e.CategoryName, "categories_category_name_key").IsUnique();

        builder.Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasColumnName("category_id");
        builder.Property(e => e.CategoryName)
            .HasMaxLength(50)
            .HasColumnName("category_name");
    }
}