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
using Scribble.Content.Web.Features.Commands.Posts;
using Scribble.Content.Web.Features.Queries;
using Scribble.Content.Web.Features.Queries.Posts;
using Scribble.Content.Web.Models.Entities;
using Scribble.Responses;

namespace Scribble.Content.Web.Controllers;

[ApiController, ApiVersion("1.0")]
[Route("api/{version:apiVersion}/posts")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PostsWritableController : UnitOfWorkWritableController<PostEntity, Guid, PostViewModel>
{
    public PostsWritableController(IMediator mediator) : base(mediator) { }

    [HttpGet("tags/{id:guid}/paged"), AllowAnonymous]
    [ProducesResponseType(typeof(ApiValidationFailureResponse<ValidationFailure>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResultResponse<IPagedCollection<PostEntity>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IApiResponse>> GetPostPagedByTagIdAsync(Guid id, [FromQuery] PaginationQueryParameters parameters)
    {
        try
        {
            var posts = await Mediator.Send(
                    new GetEntityPagedQuery<PostEntity, Guid>(parameters, x => x.Tags.Any(i => i.Id == id)),
                    HttpContext.RequestAborted)
                .ConfigureAwait(false);

            return Ok(!posts.Entities.Any()
                ? new ApiResultResponse<IPagedCollection<PostEntity>>(posts, ApiResponseDefaultMessages.NoEntityWasFound)
                : new ApiResultResponse<IPagedCollection<PostEntity>>(posts,
                    ApiResponseDefaultMessages.ResponseIsSuccessful));
        }
        catch (ValidationException exp)
        {
            return BadRequest(new ApiValidationFailureResponse<ValidationFailure>(exp.Errors, ApiResponseDefaultMessages.ValidationErrors));
        }
    }
    
    [HttpGet("tags/{tagName:required}/paged"), AllowAnonymous]
    [ProducesResponseType(typeof(ApiValidationFailureResponse<ValidationFailure>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResultResponse<IPagedCollection<PostEntity>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IApiResponse>> GetPostPagedByTagNameAsync(string tagName, [FromQuery] PaginationQueryParameters parameters)
    {
        try
        {
            var posts = await Mediator.Send(
                    new GetEntityPagedQuery<PostEntity, Guid>(parameters, x => x.Tags.Any(i => i.TagName == tagName)),
                    HttpContext.RequestAborted)
                .ConfigureAwait(false);

            return Ok(!posts.Entities.Any()
                ? new ApiResultResponse<IPagedCollection<PostEntity>>(posts, ApiResponseDefaultMessages.NoEntityWasFound)
                : new ApiResultResponse<IPagedCollection<PostEntity>>(posts,
                    ApiResponseDefaultMessages.ResponseIsSuccessful));
        }
        catch (ValidationException exp)
        {
            return BadRequest(new ApiValidationFailureResponse<ValidationFailure>(exp.Errors, ApiResponseDefaultMessages.ValidationErrors));
        }
    }

    [HttpGet("pending/all")]
    [ProducesResponseType(typeof(ApiResultResponse<IEnumerable<PostEntity>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IApiResponse>> GetAllPendingPostsAsync()
    {
        var posts = await Mediator.Send(new GetPendingPostsQuery(), HttpContext.RequestAborted)
            .ConfigureAwait(false);

        if (posts.Any())
            return Ok(new ApiResultResponse<IEnumerable<PostEntity>>(posts,
                ApiResponseDefaultMessages.ResponseIsSuccessful));

        return Ok(new ApiResultResponse<IEnumerable<PostEntity>>(posts, ApiResponseDefaultMessages.NoEntityWasFound,
            StatusCodes.Status204NoContent));
    }

    [HttpPost("pending/{key:guid}/publish")]
    [ProducesResponseType(typeof(ApiValidationFailureResponse<ValidationFailure>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<IApiResponse>> PublishPostAsync(Guid key)
    {
        try
        {
            await Mediator.Send(new PublishPostCommand(key), HttpContext.RequestAborted)
                .ConfigureAwait(false);

            return Ok(new ApiResponse(ApiResponseDefaultMessages.ResponseIsSuccessful, StatusCodes.Status204NoContent));
        }
        catch (ValidationException exp)
        {
            return BadRequest(new ApiValidationFailureResponse<ValidationFailure>(exp.Errors,
                ApiResponseDefaultMessages.ValidationErrors));
        }
    }

    [HttpGet("{postKey:guid}/views"), AllowAnonymous]
    [ProducesResponseType(typeof(ApiResultResponse<int>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IApiResponse>> GetViewsCountAsync(Guid postKey)
    {
        var viewsCount = await Mediator.Send(new GetViewsCountQuery(postKey), HttpContext.RequestAborted)
            .ConfigureAwait(false);

        return Ok(new ApiResultResponse<int>(viewsCount, ApiResponseDefaultMessages.ResponseIsSuccessful));
    }

    [HttpPost("{postKey:guid}/views")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<IApiResponse>> SetViewsCountAsync(Guid postKey)
    {
        await Mediator.Send(new SetViewsCountCommand(postKey), HttpContext.RequestAborted)
            .ConfigureAwait(false);

        return Ok(new ApiResponse(ApiResponseDefaultMessages.ResponseIsSuccessful, StatusCodes.Status204NoContent));
    }
}