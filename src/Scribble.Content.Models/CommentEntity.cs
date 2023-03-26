using Scribble.Shared.Models;

namespace Scribble.Content.Models;

public sealed class CommentEntity : Entity<Guid>
{
    public string Content { get; set; } = null!;
    public Guid PostId { get; set; }
    public Guid AuthorId { get; set; }
    public PostEntity PostEntity { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
