using Calabonga.AspNetCore.AppDefinitions;

namespace Scribble.Content.Web.Definitions.Middleware;

public class ErrorHandlingDefinition : AppDefinition
{
    public override void ConfigureApplication(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseMiddleware<ErrorHandlingMiddleware>();
    }
}