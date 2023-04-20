using System.Net.Mime;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Scribble.Content.Infrastructure.UnitOfWork.Pagination;
using Scribble.Content.Web.Definitions.Documentation;
using Scribble.Content.Web.Definitions.Documentation.Conventions;
using Scribble.Content.Web.Features.Queries;
using Scribble.Responses;
using Scribble.Shared.Models;

namespace Scribble.Content.Web.Controllers.Base;

[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class UnitOfWorkReadOnlyController<TEntity, TKey> : ControllerBase
    where TEntity : Entity<TKey> where TKey : IEquatable<TKey>
{
    protected readonly IMediator Mediator;
    public UnitOfWorkReadOnlyController(IMediator mediator) 
        => Mediator = mediator;
    
    [HttpGet("all"), AllowAnonymous, EnableQuery]
    [ProducesResponseType(typeof(ApiResultResponse<IEnumerable<ResponseEntityConventionStub>>),StatusCodes.Status200OK)]
    public virtual async Task<ActionResult<IApiResponse>> GetAllEntitiesAsync()
    {
        var entities = await Mediator.Send(new GetAllEntitiesQuery<TEntity, TKey>(), HttpContext.RequestAborted)
            .ConfigureAwait(false);
        
        return Ok(entities.Any()
            ? new ApiResultResponse<IEnumerable<TEntity>>(entities, ApiResponseDefaultMessages.NoEntityWasFound)
            : new ApiResultResponse<IEnumerable<TEntity>>(entities,
                ApiResponseDefaultMessages.ResponseIsSuccessful));
    }

    [HttpGet("{id:required}"), AllowAnonymous]
    [ProducesResponseType(typeof(ApiValidationFailureResponse<ValidationFailure>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResultResponse<ResponseEntityConventionStub>), StatusCodes.Status200OK)]
    public virtual async Task<ActionResult<IApiResponse>> GetEntityByIdAsync([FromRoute] TKey id)
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
    [ProducesResponseType(typeof(ApiResultResponse<IPagedCollection<ResponseEntityConventionStub>>), StatusCodes.Status200OK)]
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