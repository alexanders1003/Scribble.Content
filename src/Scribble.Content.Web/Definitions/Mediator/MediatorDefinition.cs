using Calabonga.AspNetCore.AppDefinitions;
using MediatR;

namespace Scribble.Content.Web.Definitions.Mediator;

public class MediatorDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddMediatR(typeof(Program).Assembly);
        services.AddGenericRequests();
    }
}