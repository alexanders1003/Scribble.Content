using Scribble.Shared.Models;

namespace Scribble.Content.Models;

public class ArticleEntity : Entity
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime PublishedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsPublished { get; set; }
    public BlogEntity Blog { get; set; } = null!;
    public ICollection<TagEntity> Tags { get; set; } = null!;
    public ICollection<CategoryEntity> Categories { get; set; } = null!;
    public ICollection<CommentEntity> Comments { get; set; } = null!;
}