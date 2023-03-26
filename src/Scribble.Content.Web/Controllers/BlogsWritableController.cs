using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scribble.Content.Infrastructure.UnitOfWork.Pagination;
using Scribble.Content.Models;
using Scribble.Content.Web.Controllers.Base;
using Scribble.Content.Web.Definitions.Documentation;
using Scribble.Content.Web.Features.Queries;
using Scribble.Content.Web.Models;
using Scribble.Content.Web.Models.Entities;
using Scribble.Responses;

namespace Scribble.Content.Web.Controllers;

[ApiController, ApiVersion("1.0")]
[Route("api/{version:apiVersion}/blogs")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BlogsWritableController : UnitOfWorkWritableController<BlogEntity, Guid, BlogViewModel>
{
    public BlogsWritableController(IMediator mediator) : base(mediator) { }

    [HttpGet("author/{id:guid}/paged"), AllowAnonymous]
    [ProducesResponseType(typeof(ApiValidationFailureResponse<ValidationFailure>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResultResponse<IPagedCollection<BlogEntity>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IApiResponse>> GetBlogPagedByAuthorIdAsync([FromRoute] Guid id, [FromQuery] PaginationQueryParameters parameters)
    {
        try
        {
            var entities = await Mediator.Send(new GetEntityPagedQuery<BlogEntity, Guid>(parameters, 
                        x => x.AuthorId == id), HttpContext.RequestAborted)
                .ConfigureAwait(false);

            return Ok(!entities.Entities.Any()
                ? new ApiResultResponse<IPagedCollection<BlogEntity>>(entities, ApiResponseDefaultMessages.NoEntityWasFound)
                : new ApiResultResponse<IPagedCollection<BlogEntity>>(entities,
                    ApiResponseDefaultMessages.ResponseIsSuccessful));
        }
        catch (ValidationException exp)
        {
            return new ApiValidationFailureResponse<ValidationFailure>(exp.Errors, ApiResponseDefaultMessages.ValidationErrors);
        }
    }

    // [HttpGet("{blogKey:guid}/followers/count")]
    // public async Task<ActionResult<IApiResponse>> GetFollowersCountAsync([FromRoute] Guid blogKey)
    // {
    //     var followersCount = await Mediator.Send(new GetFollowersCountQuery(blogKey), HttpContext.RequestAborted)
    //         .ConfigureAwait(false)
    // }
}