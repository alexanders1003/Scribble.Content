using Scribble.Shared.Models;

namespace Scribble.Content.Models;

public class CommentEntity : Entity
{
    public string Text { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime PublishedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid AuthorId { get; set; }
    public ArticleEntity Article { get; set; } = null!;
}