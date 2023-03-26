using System.Net.Mime;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Scribble.Content.Web.Definitions.Documentation;
using Scribble.Content.Web.Definitions.Documentation.Conventions;
using Scribble.Content.Web.Features.Commands;
using Scribble.Content.Web.Features.Queries;
using Scribble.Content.Web.Models.Base;
using Scribble.Responses;
using Scribble.Shared.Models;

namespace Scribble.Content.Web.Controllers.Base;

public class UnitOfWorkWritableController<TEntity, TKey, TViewModel> : UnitOfWorkReadOnlyController<TEntity, TKey>
    where TEntity : Entity<TKey> where TKey : IEquatable<TKey>
    where TViewModel : ViewModel
{
    public UnitOfWorkWritableController(IMediator mediator) 
        : base(mediator) { }

    [HttpPost]
    [ProducesResponseType(typeof(ApiValidationFailureResponse<ValidationFailure>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResultResponse<ResponseEntityConventionStub>), StatusCodes.Status201Created)]
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