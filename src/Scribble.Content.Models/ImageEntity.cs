using Scribble.Shared.Models;

namespace Scribble.Content.Models;

public class ImageEntity : Entity<Guid>
{
    public byte[] Data { get; set; } = null!;
    public string ContentType { get; set; } = null!;
}