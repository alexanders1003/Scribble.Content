namespace Scribble.Content.Web.Models.Entities;

public class BlogViewModel : ViewModel
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Guid AuthorId { get; set; }
}