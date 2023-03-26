using Scribble.Shared.Models;

namespace Scribble.Content.Models;

public sealed class CategoryEntity : Entity<Guid>
{
    public string CategoryName { get; set; } = null!;
    public IEnumerable<BlogEntity> Blogs { get; } = new List<BlogEntity>();
}
