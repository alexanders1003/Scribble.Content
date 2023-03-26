using System.Net.Mime;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Scribble.Content.Infrastructure.UnitOfWork.Pagination;
using Scribble.Content.Web.Features.Commands;
using Scribble.Content.Web.Features.Queries;
using Scribble.Content.Web.Models.Base;
using Scribble.Responses;
using Scribble.Shared.Models;

namespace Scribble.Content.Web.Controllers.Base;

[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public abstract class UnitOfWorkReadOnlyController<TEntity, TKey> : ControllerBase
    where TEntity : Entity<TKey> where TKey : IEquatable<TKey>
{
    protected readonly IMediator Mediator;
    protected UnitOfWorkReadOnlyController(IMediator mediator) 
        => Mediator = mediator;
    
    [HttpGet("all"), AllowAnonymous]
    [ProducesResponseType(typeof(ApiResultResponse<ICollection<EntityConventionStub>>),StatusCodes.Status200OK)]
    public virtual async Task<ActionResult<IApiResponse>> GetAllEntitiesAsync()
    {
        var entities = await Mediator.Send(new GetAllEntitiesQuery<TEntity, TKey>(), HttpContext.RequestAborted)
            .ConfigureAwait(false);

        return Ok(!entities.Any()
            ? new ApiResultResponse<ICollection<TEntity>>(entities, ApiResponseDefaultMessages.NoEntityWasFound)
            : new ApiResultResponse<ICollection<TEntity>>(entities,
                ApiResponseDefaultMessages.ResponseIsSuccessful));
    }

    [HttpGet("{id:required}"), AllowAnonymous]
    [ProducesResponseType(typeof(ApiValidationFailureResponse<ValidationFailure>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResultResponse<EntityConventionStub>), StatusCodes.Status200OK)]
    public virtual async Task<ActionResult<IApiResponse>> GetEntityByIdAsync(TKey id)
    {
        try
        {
            var entity = await Mediator.Send(new GetEntityByIdQuery<TEntity, TKey>(id), HttpContext.RequestAborted)
                .ConfigureAwait(false);

            return Ok(entity is null  
                ? new ApiResultResponse<TEntity?>(entity, ApiResponseDefaultMessages.EntityWasNotFound) 
                : new ApiResultResponse<TEntity?>(entity, 
                ApiResponseDefaultMessages.ResponseIsSuccessful));
        }
        catch (ValidationException exp)
        {
            return BadRequest(new ApiValidationFailureResponse<ValidationFailure>(exp.Errors, ApiResponseDefaultMessages.ValidationErrors));
        }
    }
    
    [HttpGet("paged"), AllowAnonymous]
    [ProducesResponseType(typeof(ApiValidationFailureResponse<ValidationFailure>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResultResponse<IPagedCollection<EntityConventionStub>>), StatusCodes.Status200OK)]
    public virtual async Task<ActionResult<IApiResponse>> GetPagedEntitiesAsync([FromQuery] PaginationQueryParameters parameters)
    {
        try
        {
            var entities = await Mediator.Send(new GetEntityPagedQuery<TEntity, TKey>(parameters), HttpContext.RequestAborted)
                .ConfigureAwait(false);
            
            return Ok(!entities.Entities.Any() 
                ? new ApiResultResponse<IPagedCollection<TEntity>>(entities, ApiResponseDefaultMessages.NoEntityWasFound) 
                : new ApiResultResponse<IPagedCollection<TEntity>>(entities, 
                    ApiResponseDefaultMessages.ResponseIsSuccessful));
        }
        catch (ValidationException exp)
        {
            return BadRequest(new ApiValidationFailureResponse<ValidationFailure>(exp.Errors, ApiResponseDefaultMessages.ValidationErrors));
        }
    }
}

[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public abstract class UnitOfWorkController<TEntity, TKey, TViewModel> : ControllerBase 
    where TEntity : Entity<TKey> 
    where TKey : IEquatable<TKey>
    where TViewModel : ViewModel
{
    protected readonly IMediator Mediator;
    protected UnitOfWorkController(IMediator mediator) 
        => Mediator = mediator;

    [HttpPost]
    [ProducesResponseType(typeof(ApiValidationFailureResponse<ValidationFailure>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResultResponse<EntityConventionStub>), StatusCodes.Status201Created)]
    public virtual async Task<ActionResult<IApiResponse>> CreateEntityAsync([FromBody] TViewModel viewModel)
    {
        try
        {
            var entity = await Mediator.Send(new CreateEntityCommand<TEntity, TKey, TViewModel>(viewModel), HttpContext.RequestAborted)
                .ConfigureAwait(false);

            var response = new ApiResultResponse<TEntity>(entity, ApiResponseDefaultMessages.EntityWasSuccessfullyCreated,
                StatusCodes.Status201Created);

            return Created(HttpContext.Request.GetDisplayUrl(), response);
        }
        catch (ValidationException exp)
        {
            return BadRequest(new ApiValidationFailureResponse<ValidationFailure>(exp.Errors, ApiResponseDefaultMessages.ValidationErrors));
        }
    }
    
    [HttpPut("{key:required}")]
    [ProducesResponseType(typeof(ApiValidationFailureResponse<ValidationFailure>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status200OK)]
    public virtual async Task<ActionResult<IApiResponse>> UpdateEntityAsync(TKey key, TViewModel viewModel)
    {
        try
        {
            await Mediator.Send(new UpdateEntityCommand<TEntity, TKey, TViewModel>(key, viewModel), 
                    HttpContext.RequestAborted)
                .ConfigureAwait(false);

            return Ok(new ApiResponse(ApiResponseDefaultMessages.EntityWasSuccessfullyUpdated, StatusCodes.Status204NoContent));
        }
        catch (ValidationException exp)
        {
            return BadRequest(new ApiValidationFailureResponse<ValidationFailure>(exp.Errors, ApiResponseDefaultMessages.ValidationErrors));
        }
    }
    
    [HttpDelete("{key:required}")]
    [ProducesResponseType(typeof(ApiValidationFailureResponse<ValidationFailure>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public virtual async Task<ActionResult<IApiResponse>> DeleteEntityAsync(TKey key)
    {
        try
        {
            var entity = await Mediator.Send(new GetEntityByIdQuery<TEntity, TKey>(key), HttpContext.RequestAborted)
                .ConfigureAwait(false);

            if (entity is null)
                return Ok(new ApiResponse(ApiResponseDefaultMessages.EntityWasNotFound, 
                    StatusCodes.Status204NoContent));

            await Mediator.Send(new RemoveEntityCommand<TEntity, TKey>(key), HttpContext.RequestAborted)
                .ConfigureAwait(false);

            return Ok(new ApiResponse(ApiResponseDefaultMessages.EntityWasSuccessfullyDeleted,
                StatusCodes.Status204NoContent));
        }
        catch (ValidationException exp)
        {
            return BadRequest(new ApiValidationFailureResponse<ValidationFailure>(exp.Errors, ApiResponseDefaultMessages.ValidationErrors));
        }
    }
}