using Scribble.Content.Web.Models.Base;

namespace Scribble.Content.Web.Models.Entities;

public class LikeViewModel : ViewModel
{
    public Guid UserId { get; set; }
    public Guid PostId { get; set; }
}