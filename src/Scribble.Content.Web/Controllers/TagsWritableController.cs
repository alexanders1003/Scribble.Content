using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scribble.Content.Models;
using Scribble.Content.Web.Controllers.Base;
using Scribble.Content.Web.Definitions.Documentation;
using Scribble.Content.Web.Features.Queries;
using Scribble.Content.Web.Models;
using Scribble.Content.Web.Models.Entities;
using Scribble.Responses;

namespace Scribble.Content.Web.Controllers;

[ApiController, ApiVersion("1.0")]
[Route("api/{version:apiVersion}/tags")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TagsWritableController : UnitOfWorkWritableController<TagEntity, Guid, TagViewModel>
{
    public TagsWritableController(IMediator mediator) : base(mediator) { }
    
    [HttpGet("post/{key:guid}/all"), AllowAnonymous]
    [ProducesResponseType(typeof(ApiValidationFailureResponse<ValidationFailure>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<TagEntity>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IApiResponse>> GetAllTagsByArticleIdAsync(Guid key)
    {
        try
        {
            var tags = await Mediator.Send(new GetAllEntitiesQuery<TagEntity, Guid>(
                        x => x.Posts.Any(i => i.Id == key)), HttpContext.RequestAborted)
                .ConfigureAwait(false);

            return Ok(!tags.Any()
                ? new ApiResultResponse<IEnumerable<TagEntity>>(tags, ApiResponseDefaultMessages.NoEntityWasFound)
                : new ApiResultResponse<IEnumerable<TagEntity>>(tags, ApiResponseDefaultMessages.ResponseIsSuccessful));
        }
        catch (ValidationException exp)
        {
            return new ApiValidationFailureResponse<ValidationFailure>(exp.Errors, ApiResponseDefaultMessages.ValidationErrors);
        }
    }

    [HttpPut, NonAction]
    public override Task<ActionResult<IApiResponse>> UpdateEntityAsync(Guid key, TagViewModel entity)
    {
        throw new NotSupportedException(ApiResponseDefaultMessages.MethodNotAllowedForEntity);
    }
}