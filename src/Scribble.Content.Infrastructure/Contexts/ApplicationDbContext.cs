using Microsoft.EntityFrameworkCore;
using Scribble.Content.Models;

namespace Scribble.Content.Infrastructure.Contexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext() { }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<BlogEntity> Blogs { get; set; } = null!;

    public DbSet<CategoryEntity> Categories { get; set; } = null!;

    public DbSet<CommentEntity> Comments { get; set; } = null!;

    public DbSet<FollowerEntity> Followers { get; set; } = null!;

    public DbSet<LikeEntity> Likes { get; set; } = null!;

    public DbSet<PostEntity> Posts { get; set; } = null!;

    public DbSet<TagEntity> Tags { get; set; } = null!;

    public DbSet<ImageEntity> Images { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
