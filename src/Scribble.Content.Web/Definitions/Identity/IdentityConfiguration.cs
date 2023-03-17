namespace Scribble.Content.Web.Definitions.Identity;

public class IdentityConfiguration
{
    public string Authority { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
}