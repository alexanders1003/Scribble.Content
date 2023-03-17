using Scribble.Shared.Models;

namespace Scribble.Content.Models;

public class CategoryEntity : Entity
{
    public string Name { get; set; } = null!;
    public ICollection<ArticleEntity> Articles { get; set; } = null!;
}