using Scribble.Shared.Models;

namespace Scribble.Content.Models;

public sealed class LikeEntity : Entity<Guid>
{
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public PostEntity PostEntity { get; set; } = null!;
}
