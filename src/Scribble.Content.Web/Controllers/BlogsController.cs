using AutoWrapper.Models.ResponseTypes;
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
[Route("api/{version:apiVersion}/blogs")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BlogsController : UnitOfWorkController<BlogEntity>
{
    public BlogsController(IMediator mediator) : base(mediator) { }

    [HttpGet("author/{id:guid}/paged"), AllowAnonymous]
    [ProducesResponseType(typeof(ApiResultResponse<>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetailsValidationErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IPagedCollection<BlogEntity>> GetPagedByAuthorIdAsync(Guid id, [FromQuery] PaginationQueryParameters parameters)
    {
        return await Mediator.Send(new GetBlogPagedByAuthorIdQuery(id, parameters), HttpContext.RequestAborted)
            .ConfigureAwait(false);
    }
}