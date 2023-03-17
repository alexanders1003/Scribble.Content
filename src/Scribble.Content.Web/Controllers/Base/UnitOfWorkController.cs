using System.Net.Mime;
using AutoWrapper.Models;
using AutoWrapper.Models.ResponseTypes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scribble.Content.Infrastructure.UnitOfWork.Pagination;
using Scribble.Content.Web.Features.Commands;
using Scribble.Content.Web.Features.Queries;
using Scribble.Shared.Models;

namespace Scribble.Content.Web.Controllers.Base;

[Consumes(MediaTypeNames.Application.Json)]
public abstract class UnitOfWorkController<TEntity> : ControllerBase 
    where TEntity : Entity
{
    protected readonly IMediator Mediator;
    protected UnitOfWorkController(IMediator mediator) 
        => Mediator = mediator;

    [HttpGet("all"), AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetailsValidationErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ApiResultResponse<ICollection<TEntity>>> GetAllEntitiesAsync()
    {
        var entities = await Mediator.Send(new GetAllEntitiesQuery<TEntity>(), HttpContext.RequestAborted)
            .ConfigureAwait(false);

        return new ApiResultResponse<ICollection<TEntity>> { Message = "Request Successful", Result = entities };
    }

    [HttpGet("{id:guid}"), AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetailsValidationErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<TEntity?> GetEntityByIdAsync(Guid id)
    {
        return await Mediator.Send(new GetEntityByIdQuery<TEntity>(id), HttpContext.RequestAborted)
            .ConfigureAwait(false);
    }
    
    [HttpGet("paged"), AllowAnonymous]
    [ProducesResponseType(typeof(ApiResultResponse<>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetailsValidationErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IPagedCollection<TEntity>> GetPagedEntitiesAsync([FromQuery] PaginationQueryParameters parameters)
    {
        return await Mediator.Send(new GetEntityPagedQuery<TEntity>(parameters), HttpContext.RequestAborted)
            .ConfigureAwait(false);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ApiResultResponse<>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiProblemDetailsValidationErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<TEntity> CreateEntityAsync(TEntity model)
    {
        return await Mediator.Send(new CreateEntityCommand<TEntity>(model), HttpContext.RequestAborted)
            .ConfigureAwait(false);
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(ApiResultResponse<>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiProblemDetailsValidationErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task UpdateEntityAsync(TEntity entity)
    {
        await Mediator.Send(new UpdateEntityCommand<TEntity>(entity), HttpContext.RequestAborted)
            .ConfigureAwait(false);
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResultResponse<>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiProblemDetailsValidationErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task DeleteEntityAsync(Guid id)
    {
        var entity = await Mediator.Send(new GetEntityByIdQuery<TEntity>(id), HttpContext.RequestAborted)
            .ConfigureAwait(false);

        if (entity is not null)
            await Mediator.Send(new RemoveEntityCommand<TEntity>(entity), HttpContext.RequestAborted)
                .ConfigureAwait(false);
    }
}