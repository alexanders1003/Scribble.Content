using Scribble.Shared.Models;

namespace Scribble.Content.Models;

public class BlogEntity : Entity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid AuthorId { get; set; }
    public ICollection<ArticleEntity> Articles { get; set; } = null!;
}