using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scribble.Content.Models;

namespace Scribble.Content.Infrastructure.Contexts.Configuration;

public class TagEntityTypeConfiguration : IEntityTypeConfiguration<TagEntity>
{
    public void Configure(EntityTypeBuilder<TagEntity> builder)
    {
        builder.ToTable("tags").HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("tag_id")
            .HasDefaultValueSql("NEWID()");

        builder.Property(x => x.Name).HasColumnName("tag_name")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.AuthorId).HasColumnName("author_id")
            .IsRequired();
    }
}