using Scribble.Shared.Models;

namespace Scribble.Content.Models;

public class PostEntity : Entity<Guid>
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public Guid BlogId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public bool IsPosted { get; set; }
    public bool IsRejected { get; set; }
    public int ViewsCount { get; set; }
    public virtual BlogEntity BlogEntity { get; set; } = null!;
    public virtual IEnumerable<CommentEntity> Comments { get; } = new List<CommentEntity>();
    public virtual IEnumerable<LikeEntity> Likes { get; } = new List<LikeEntity>();
    public virtual IEnumerable<TagEntity> Tags { get; } = new List<TagEntity>();
}
