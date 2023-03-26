using System.Reflection;
using AutoMapper.Internal;
using MediatR;
using Scribble.Content.Web.Controllers.Base;

namespace Scribble.Content.Web.Definitions.Mediator;

public static class MediatorServiceCollectionExtensions
{
    public static void AddGenericRequests(this IServiceCollection services)
    {
        foreach (var controller in FindUnitOfWorkControllers())
        {
            var controllerGenericArgs = controller.BaseType!.GetGenericArguments();
            
            foreach (var handler in FindGenericRequestHandlers())
            {
                var genericArgs = controllerGenericArgs.Take(handler.GetGenericArguments().Length).ToArray();

                var genericHandler = handler.MakeGenericType(genericArgs);
                var genericInterface = genericHandler.GetInterfaces()
                    .First(x => x.IsGenericType && (x.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)
                                || x.GetGenericTypeDefinition() == typeof(IRequestHandler<>)));

                services.AddScoped(genericInterface, genericHandler);
            }
        }
    }

    private static IEnumerable<Type> FindUnitOfWorkControllers()
    {
        return Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => x.BaseType is { IsGenericType: true } &&
                        x.BaseType.GetGenericTypeDefinition() == typeof(UnitOfWorkWritableController<,,>))
            .ToArray();
    }

    private static IEnumerable<Type> FindGenericRequestHandlers()
    {
        return Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => x.IsGenericType && x.GetInterfaces().Any(
                i => i.IsGenericType && (i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>) ||
                     i.GetGenericTypeDefinition() == typeof(IRequestHandler<>))))
            .ToArray();
    }
}