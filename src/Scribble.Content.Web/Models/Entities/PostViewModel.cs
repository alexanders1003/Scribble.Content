using Scribble.Content.Web.Models.Base;

namespace Scribble.Content.Web.Models.Entities;

public class PostViewModel : ViewModel
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public Guid BlogId { get; set; }
}