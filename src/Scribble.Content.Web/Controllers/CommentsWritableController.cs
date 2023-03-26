using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scribble.Content.Infrastructure.UnitOfWork.Pagination;
using Scribble.Content.Models;
using Scribble.Content.Web.Controllers.Base;
using Scribble.Content.Web.Features.Queries;
using Scribble.Content.Web.Models;
using Scribble.Responses;

namespace Scribble.Content.Web.Controllers;

[ApiController, ApiVersion("1.0")]
[Route("api/{version:apiVersion}/comments")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CommentsWritableController : UnitOfWorkWritableController<CommentEntity, Guid, CommentViewModel>
{
    public CommentsWritableController(IMediator mediator) : base(mediator) { }
    
    [HttpGet("article/{id:guid}/all"), AllowAnonymous]
    [ProducesResponseType(typeof(ApiValidationFailureResponse<ValidationFailure>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResultResponse<ICollection<CommentEntity>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IApiResponse>> GetAllCommentsByPostIdAsync(Guid id)
    {
        try
        {
            var comments = await Mediator.Send(new GetAllEntitiesQuery<CommentEntity, Guid>(x => x.Post.Id == id),
                    HttpContext.RequestAborted)
                .ConfigureAwait(false);

            return Ok(!comments.Any()
                ? new ApiResultResponse<ICollection<CommentEntity>>(comments,
                    ApiResponseDefaultMessages.NoEntityWasFound)
                : new ApiResultResponse<ICollection<CommentEntity>>(comments,
                    ApiResponseDefaultMessages.ResponseIsSuccessful));
        }
        catch (ValidationException exp)
        {
            return new ApiValidationFailureResponse<ValidationFailure>(exp.Errors, ApiResponseDefaultMessages.ValidationErrors);
        }
    }

    [HttpGet("article/{id:guid}/paged"), AllowAnonymous]
    [ProducesResponseType(typeof(ApiValidationFailureResponse<ValidationFailure>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResultResponse<IPagedCollection<CommentEntity>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IApiResponse>> GetCommentPagedByArticleIdAsync(Guid id, 
        PaginationQueryParameters parameters)
    {
        try
        {
            var comments = await Mediator.Send(new GetEntityPagedQuery<CommentEntity, Guid>(parameters, x => x.Post.Id == id), 
                    HttpContext.RequestAborted)
                .ConfigureAwait(false);
                
            return Ok(!comments.Entities.Any()
                ? new ApiResultResponse<IPagedCollection<CommentEntity>>(comments,
                    ApiResponseDefaultMessages.NoEntityWasFound)
                : new ApiResultResponse<IPagedCollection<CommentEntity>>(comments,
                    ApiResponseDefaultMessages.ResponseIsSuccessful));
        }
        catch (ValidationException exp)
        {
            return new ApiValidationFailureResponse<ValidationFailure>(exp.Errors, ApiResponseDefaultMessages.ValidationErrors);
        }
    }
}