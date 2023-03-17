﻿using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scribble.Content.Models;
using Scribble.Content.Web.Controllers.Base;
using Scribble.Content.Web.Features.Queries;

namespace Scribble.Content.Web.Controllers;

[ApiController, ApiVersion("1.0")]
[Route("api/{version:apiVersion}/categories")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CategoriesController : UnitOfWorkController<CategoryEntity>
{
    public CategoriesController(IMediator mediator) : base(mediator) { }

    [HttpGet("article/{id:guid}/all"), AllowAnonymous]
    [ProducesResponseType(typeof(OkObjectResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ICollection<CategoryEntity>>> GetCategoriesByBlogIdAsync(Guid id)
    {
        var collection = await Mediator.Send(new GetAllCategoriesByArticleIdQuery(id), 
                HttpContext.RequestAborted)
            .ConfigureAwait(false);

        return new OkObjectResult(collection);
    }
}