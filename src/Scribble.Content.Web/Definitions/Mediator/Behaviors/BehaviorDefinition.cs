using Calabonga.AspNetCore.AppDefinitions;
using MediatR;

namespace Scribble.Content.Web.Definitions.Mediator.Behaviors;

public class BehaviorDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    }
}