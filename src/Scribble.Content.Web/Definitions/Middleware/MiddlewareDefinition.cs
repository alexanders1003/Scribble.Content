using Calabonga.AspNetCore.AppDefinitions;

namespace Scribble.Content.Web.Definitions.Middleware;

public class MiddlewareDefinition : AppDefinition
{
    public override void ConfigureApplication(WebApplication app)
    {
        app.UseMiddleware<ValidationExceptionHandlerMiddleware>();
    }
}