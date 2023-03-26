using Scribble.Content.Web.Models.Base;

namespace Scribble.Content.Web.Models.Entities;

public class CommentViewModel : ViewModel
{
    public string? Text { get; set; }
    public Guid PostId { get; set; }
    public Guid AuthorId { get; set; }
}