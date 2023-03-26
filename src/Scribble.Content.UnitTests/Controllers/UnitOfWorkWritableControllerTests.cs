using AutoFixture;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Scribble.Content.Web.Controllers.Base;
using Scribble.Content.Web.Features.Commands;
using Scribble.Content.Web.Models.Base;
using Scribble.Responses;
using Scribble.Shared.Models;
using Xunit;

namespace Scribble.Content.UnitTests.Controllers;

public abstract class UnitOfWorkWritableControllerTests<TEntity, TKey, TViewModel> : UnitOfWorkReadOnlyControllerTests<TEntity, TKey> 
    where TEntity : Entity<TKey> where TKey : IEquatable<TKey> where TViewModel : ViewModel
{
    [Fact]
    public virtual async Task CreateEntity_WhenViewModelIsValid_ReturnsStatusCode200()
    {
        var viewModel = Fixture.Create<TViewModel>(); 
        
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(x => 
                x.Send(It.IsAny<CreateEntityCommand<TEntity, TKey, TViewModel>>(), It.IsAny<CancellationToken>()))!
            .ReturnsAsync(It.IsAny<TEntity>());

        var controller = FakeControllerFactory
            .Create<UnitOfWorkWritableController<TEntity, TKey, TViewModel>>(mediatorMock.Object);

        var actionResult = await controller.CreateEntityAsync(viewModel);

        var createdResult = Assert.IsType<CreatedResult>(actionResult.Result);
        var apiResultResponse = Assert.IsType<ApiResultResponse<TEntity>>(createdResult.Value);
        Assert.Equal(StatusCodes.Status201Created, apiResultResponse.StatusCode);
    }
    
    [Fact]
    public virtual async Task CreateEntity_WhenViewModelIsNull_ReturnsStatusCode400()
    {
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(x => 
                x.Send(It.IsAny<CreateEntityCommand<TEntity, TKey, TViewModel>>(), It.IsAny<CancellationToken>()))!
            .ReturnsAsync(It.IsAny<TEntity>());

        var controller = FakeControllerFactory
            .Create<UnitOfWorkWritableController<TEntity, TKey, TViewModel>>(mediatorMock.Object);

        var actionResult = await controller.CreateEntityAsync(null!);

        var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        var apiValidationFailureResponse = Assert.IsType<ApiValidationFailureResponse<ValidationFailure>>(badRequestObjectResult.Value);
        Assert.Equal(StatusCodes.Status400BadRequest, apiValidationFailureResponse.StatusCode);
    }

    [Fact]
    public virtual async Task UpdateEntity_WhenViewModelIsValid_ReturnsStatusCode200()
    {
        var viewModel = Fixture.Create<TViewModel>();
        
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(x => 
                x.Send(It.IsAny<UpdateEntityCommand<TEntity, TKey, TViewModel>>(), It.IsAny<CancellationToken>()))!
            .ReturnsAsync(It.IsAny<Unit>());

        var controller = FakeControllerFactory
            .Create<UnitOfWorkWritableController<TEntity, TKey, TViewModel>>(mediatorMock.Object);

        var actionResult = await controller.UpdateEntityAsync(It.IsAny<TKey>(), viewModel);

        var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var apiResponse = Assert.IsType<ApiResponse>(okObjectResult.Value);
        Assert.Equal(StatusCodes.Status204NoContent, apiResponse.StatusCode);
    }
    
    [Fact]
    public virtual async Task UpdateEntity_WhenViewModelIsNull_ReturnsStatusCode400()
    {
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(x => 
                x.Send(It.IsAny<UpdateEntityCommand<TEntity, TKey, TViewModel>>(), It.IsAny<CancellationToken>()))!
            .ReturnsAsync(It.IsAny<Unit>());

        var controller = FakeControllerFactory
            .Create<UnitOfWorkWritableController<TEntity, TKey, TViewModel>>(mediatorMock.Object);

        var actionResult = await controller.UpdateEntityAsync(It.IsAny<TKey>(), null!);

        var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        var apiValidationFailureResponse = Assert.IsType<ApiValidationFailureResponse<ValidationFailure>>(badRequestObjectResult.Value);
        Assert.Equal(StatusCodes.Status400BadRequest, apiValidationFailureResponse.StatusCode);
    }

    [Fact]
    public virtual async Task DeleteEntity_ReturnsStatusCode200()
    {
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(x => 
                x.Send(It.IsAny<RemoveEntityCommand<TEntity, TKey>>(), It.IsAny<CancellationToken>()))!
            .ReturnsAsync(It.IsAny<Unit>());

        var controller = FakeControllerFactory
            .Create<UnitOfWorkWritableController<TEntity, TKey, TViewModel>>(mediatorMock.Object);

        var actionResult = await controller.DeleteEntityAsync(It.IsAny<TKey>());

        var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var apiResponse = Assert.IsType<ApiResponse>(okObjectResult.Value);
        Assert.Equal(StatusCodes.Status204NoContent, apiResponse.StatusCode);
    }
}