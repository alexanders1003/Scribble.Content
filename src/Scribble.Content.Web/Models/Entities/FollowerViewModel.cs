namespace Scribble.Content.Web.Models.Entities;

public class FollowerViewModel : ViewModel
{
    public Guid FollowerId { get; set; }
    public Guid BlogId { get; set; }
}