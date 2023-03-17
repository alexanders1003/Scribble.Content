using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scribble.Content.Infrastructure.UnitOfWork.Pagination;
using Scribble.Content.Models;
using Scribble.Content.Web.Controllers.Base;
using Scribble.Content.Web.Features.Queries;

namespace Scribble.Content.Web.Controllers;

[ApiController, ApiVersion("1.0")]
[Route("api/{version:apiVersion}/comments")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CommentsController : UnitOfWorkController<CommentEntity>
{
    public CommentsController(IMediator mediator) : base(mediator) { }
    
    [HttpGet("article/{id:guid}/all"), AllowAnonymous]
    [ProducesResponseType(typeof(OkObjectResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ICollection<CommentEntity>>> GetAllCommentsByArticleIdAsync(Guid id)
    {
        var collection = await Mediator.Send(new GetAllCommentsByArticleIdQuery(id), 
                HttpContext.RequestAborted)
            .ConfigureAwait(false);

        return new OkObjectResult(collection);
    }

    [HttpGet("article/{id:guid}/paged"), AllowAnonymous]
    [ProducesResponseType(typeof(OkObjectResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IPagedCollection<CommentEntity>>> GetCommentPagedByArticleIdAsync(Guid id, 
        PaginationQueryParameters parameters)
    {
        var collection = await Mediator.Send(new GetCommentPagedByArticleIdQuery(id, parameters), 
                HttpContext.RequestAborted)
            .ConfigureAwait(false);

        return new OkObjectResult(collection);
    }
}