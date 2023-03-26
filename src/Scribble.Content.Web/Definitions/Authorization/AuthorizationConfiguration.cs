namespace Scribble.Content.Web.Definitions.Authorization;

public class AuthorizationConfiguration
{
    public string Authority { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
}