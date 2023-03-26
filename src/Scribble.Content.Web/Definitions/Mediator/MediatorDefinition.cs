using System.Reflection;
using Calabonga.AspNetCore.AppDefinitions;
using MediatR;
using Scribble.Content.Web.Definitions.CQRS;
using Scribble.Content.Web.Features;
using Scribble.Shared.Models;

namespace Scribble.Content.Web.Definitions.Mediator;

public class MediatorDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddMediatR(typeof(Program).Assembly);

        var entities = Assembly.Load("Scribble.Content.Models").GetTypes()
            .Where(x => x.IsSubclassOf(typeof(Entity)))
            .ToArray();

        var registrars = Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => x.GetInterfaces().Contains(typeof(IRequestRegistrator)))
            .ToArray();

        foreach (var registrar in registrars)
        {
            var instance = (IRequestRegistrator)Activator.CreateInstance(registrar)!;
            instance.Register(services, entities);
        }
    }
}