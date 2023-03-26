using Scribble.Shared.Models;

namespace Scribble.Content.Models;

public sealed class FollowerEntity : Entity<Guid>
{
    public Guid BlogId { get; set; }
    public Guid FollowerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public BlogEntity BlogEntity { get; set; } = null!;
}
