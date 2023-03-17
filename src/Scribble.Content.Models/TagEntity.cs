using Scribble.Shared.Models;

namespace Scribble.Content.Models;

public class TagEntity : Entity
{
    public string Name { get; set; } = null!;
    public Guid AuthorId { get; set; }
    public ICollection<ArticleEntity> Articles { get; set; } = null!;
}