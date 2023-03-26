using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scribble.Content.Models;
using Scribble.Content.Web.Controllers.Base;
using Scribble.Content.Web.Features.Queries;
using Scribble.Content.Web.Models;
using Scribble.Responses;

namespace Scribble.Content.Web.Controllers;

[ApiController, ApiVersion("1.0")]
[Route("api/{version:apiVersion}/categories")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CategoriesWritableController : UnitOfWorkWritableController<CategoryEntity, Guid, CategoryViewModel>
{
    public CategoriesWritableController(IMediator mediator) : base(mediator) { }

    [HttpGet("article/{id:guid}/all"), AllowAnonymous]
    [ProducesResponseType(typeof(ApiValidationFailureResponse<ValidationFailure>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResultResponse<ICollection<CategoryEntity>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IApiResponse>> GetAllCategoriesByBlogIdAsync(Guid id)
    {
        try
        {
            var categories = await Mediator.Send(
                    new GetAllEntitiesQuery<CategoryEntity, Guid>(x => x.Blogs.Any(i => i.Id == id)),
                    HttpContext.RequestAborted)
                .ConfigureAwait(false);

            return Ok(!categories.Any()
                ? new ApiResultResponse<ICollection<CategoryEntity>>(categories, ApiResponseDefaultMessages.NoEntityWasFound)
                : new ApiResultResponse<ICollection<CategoryEntity>>(categories,
                    ApiResponseDefaultMessages.ResponseIsSuccessful));
        }
        catch (ValidationException exp)
        {
            return new ApiValidationFailureResponse<ValidationFailure>(exp.Errors, ApiResponseDefaultMessages.ValidationErrors);
        }
    }
}