using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scribble.Content.Models;

namespace Scribble.Content.Infrastructure.Contexts.EntityConfigurations;

public class TagEntityTypeConfiguration : IEntityTypeConfiguration<TagEntity>
{
    public void Configure(EntityTypeBuilder<TagEntity> builder)
    {
        builder.HasKey(e => e.Id).HasName("tags_pkey");

        builder.ToTable("tags");

        builder.HasIndex(e => e.TagName, "tags_tag_name_key").IsUnique();

        builder.Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasColumnName("tag_id");
        builder.Property(e => e.TagName)
            .HasMaxLength(20)
            .HasColumnName("tag_name");
    }
}