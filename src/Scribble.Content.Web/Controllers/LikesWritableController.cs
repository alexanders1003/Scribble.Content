using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scribble.Content.Models;
using Scribble.Content.Web.Controllers.Base;
using Scribble.Content.Web.Definitions.Documentation;
using Scribble.Content.Web.Models;
using Scribble.Content.Web.Models.Entities;
using Scribble.Responses;

namespace Scribble.Content.Web.Controllers;

[ApiController, ApiVersion("1.0")]
[Route("api/{version:apiVersion}/likes")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class LikesWritableController : UnitOfWorkWritableController<LikeEntity, Guid, LikeViewModel>
{
    public LikesWritableController(IMediator mediator) 
        : base(mediator) { }

    [HttpPut, NonAction]
    public override Task<ActionResult<IApiResponse>> UpdateEntityAsync(Guid key, LikeViewModel entity)
    {
        throw new NotSupportedException(ApiResponseDefaultMessages.MethodNotAllowedForEntity);
    }
}