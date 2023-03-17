using Microsoft.EntityFrameworkCore;
using Scribble.Content.Models;

namespace Scribble.Content.Infrastructure.Contexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
    
    public DbSet<BlogEntity> Pages { get; set; }
    public DbSet<ArticleEntity> Articles { get; set; }
    public DbSet<TagEntity> Tags { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<CommentEntity> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.Entity<BlogEntity>()
            .HasMany(x => x.Articles)
            .WithOne(x => x.Blog);

        modelBuilder.Entity<ArticleEntity>()
            .HasMany(x => x.Tags)
            .WithMany(x => x.Articles)
            .UsingEntity(j => j.ToTable("articles_tags"));

        modelBuilder.Entity<ArticleEntity>()
            .HasMany(x => x.Categories)
            .WithMany(x => x.Articles)
            .UsingEntity(j => j.ToTable("articles_categories"));

        modelBuilder.Entity<ArticleEntity>()
            .HasMany(x => x.Comments)
            .WithOne(x => x.Article);
    }
}