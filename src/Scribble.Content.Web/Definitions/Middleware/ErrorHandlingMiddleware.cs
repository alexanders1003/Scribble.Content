using AutoWrapper.Exceptions;
using AutoWrapper.Models;
using FluentValidation;

namespace Scribble.Content.Web.Definitions.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next) 
        => _next = next;
    
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await Handle(context, ex);
        }
    }

    private static async Task Handle(HttpContext context, Exception exp)
    {
        context.Response.ContentType = "application/json";

        switch (exp)
        {
            case ValidationException validationExp:
                throw new ApiException(validationExp.Errors.Select(x =>
                    new ValidationError(x.PropertyName, x.ErrorMessage)));
            default:
                throw exp;
        }
    }
}