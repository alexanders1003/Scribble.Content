using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scribble.Content.Infrastructure.UnitOfWork.Pagination;
using Scribble.Content.Models;
using Scribble.Content.Web.Controllers.Base;
using Scribble.Content.Web.Definitions.Swagger;
using Scribble.Content.Web.Features.Queries;

namespace Scribble.Content.Web.Controllers;

[ApiController, ApiVersion("1.0")]
[Route("api/{version:apiVersion}/articles")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ArticlesController : UnitOfWorkController<ArticleEntity>
{
    public ArticlesController(IMediator mediator) : base(mediator) { }

    [HttpGet("tags/{id:guid}/paged")]
    [ProducesResponseType(typeof(OkObjectResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IPagedCollection<ArticleEntity>>> GetPagedByTagIdAsync(Guid id, [FromQuery] PaginationQueryParameters parameters)
    {
        var collection = await Mediator
            .Send(new GetArticlePagedByTagIdQuery(id, parameters), HttpContext.RequestAborted)
            .ConfigureAwait(false);

        return new OkObjectResult(collection);
    }
}