using Scribble.Shared.Models;

namespace Scribble.Content.Models;

public sealed class BlogEntity : Entity<Guid>
{
    public string BlogName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? RemovedAt { get; set; }
    public Guid AuthorId { get; set; }
    public IEnumerable<FollowerEntity> Followers { get; } = new List<FollowerEntity>();
    public IEnumerable<PostEntity> Posts { get; } = new List<PostEntity>();
    public IEnumerable<CategoryEntity> Categories { get; } = new List<CategoryEntity>();
}
