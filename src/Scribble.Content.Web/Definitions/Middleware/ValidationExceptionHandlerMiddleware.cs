using System.Net.Mime;
using FluentValidation;
using FluentValidation.Results;
using Scribble.Responses;

namespace Scribble.Content.Web.Definitions.Middleware;

public class ValidationExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationExceptionHandlerMiddleware(RequestDelegate next) 
        => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException exp)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var validationResponse =
                new ApiValidationFailureResponse<ValidationFailure>(exp.Errors, "Validation failed.");

            await context.Response.WriteAsJsonAsync(validationResponse);
        }
    }
}