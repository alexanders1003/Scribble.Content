using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Scribble.Content.UnitTests;

public static class FakeControllerFactory
{
    public static TController Create<TController>(params object?[]? args) 
        where TController : ControllerBase
    {
        var controller = Activator.CreateInstance(typeof(TController), args) as TController;

        if (controller is null)
            throw new InvalidOperationException($"Cannot create controller of type {typeof(TController).Name}.");
        
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        return controller;
    }
}