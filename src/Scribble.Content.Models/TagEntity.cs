using Scribble.Shared.Models;

namespace Scribble.Content.Models;

public sealed class TagEntity : Entity<Guid>
{
    public string TagName { get; set; } = null!;
    public IEnumerable<PostEntity> Posts { get; } = new List<PostEntity>();
}
