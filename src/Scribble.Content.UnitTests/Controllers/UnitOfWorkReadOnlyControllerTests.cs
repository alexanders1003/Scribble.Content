using AutoFixture;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Scribble.Content.Infrastructure.UnitOfWork.Pagination;
using Scribble.Content.Web.Controllers.Base;
using Scribble.Content.Web.Features.Queries;
using Scribble.Responses;
using Scribble.Shared.Models;
using Xunit;

namespace Scribble.Content.UnitTests.Controllers;

public abstract class UnitOfWorkReadOnlyControllerTests<TEntity, TKey> 
    where TEntity : Entity<TKey> where TKey : IEquatable<TKey>
{
    protected readonly Fixture Fixture = new();
    
    [Fact]
    public virtual async Task GetAllEntities_ReturnsStatusCode200()
    {
        var entities = Fixture.CreateMany<TEntity>(10); 
        
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(x => 
                x.Send(It.IsAny<GetAllEntitiesQuery<TEntity, TKey>>(), It.IsAny<CancellationToken>()))!
            .ReturnsAsync(entities);

        var controller = FakeControllerFactory
            .Create<UnitOfWorkReadOnlyController<TEntity, TKey>>(mediatorMock.Object);

        var actionResult = await controller.GetAllEntitiesAsync();

        var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var apiResultResponse = Assert.IsType<ApiResultResponse<IEnumerable<TEntity>>>(okObjectResult.Value);
        Assert.Equal(StatusCodes.Status200OK, apiResultResponse.StatusCode);
    }

    [Fact]
    public virtual async Task GetEntityById_ReturnsStatusCode200()
    {
        var entity = Fixture.Create<TEntity>(); 
        
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(x => 
                x.Send(It.IsAny<GetEntityByIdQuery<TEntity, TKey>>(), It.IsAny<CancellationToken>()))!
            .ReturnsAsync(entity);

        var controller = FakeControllerFactory
            .Create<UnitOfWorkReadOnlyController<TEntity, TKey>>(mediatorMock.Object);

        var actionResult = await controller.GetEntityByIdAsync(entity.Id);

        var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var apiResultResponse = Assert.IsType<ApiResultResponse<TEntity>>(okObjectResult.Value);
        Assert.Equal(StatusCodes.Status200OK, apiResultResponse.StatusCode);
    }

    [Fact]
    public virtual async Task GetPagedEntities_WhenPaginationQueryParametersIsValid_ReturnsStatusCode200()
    {
        var paginationQueryParameters = new PaginationQueryParameters { IndexFrom = 0, PageIndex = 0, PageSize = 5 };
        var entities = Fixture.CreateMany<TEntity>(10);
        var pagedCollection = new PagedCollection<TEntity>(entities, paginationQueryParameters);
        
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(x => 
                x.Send(It.IsAny<GetEntityPagedQuery<TEntity, TKey>>(), It.IsAny<CancellationToken>()))!
            .ReturnsAsync(pagedCollection);

        var controller = FakeControllerFactory
            .Create<UnitOfWorkReadOnlyController<TEntity, TKey>>(mediatorMock.Object);

        var actionResult = await controller.GetPagedEntitiesAsync(paginationQueryParameters);

        var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var apiResultResponse = Assert.IsType<ApiResultResponse<IPagedCollection<TEntity>>>(okObjectResult.Value);
        Assert.Equal(StatusCodes.Status200OK, apiResultResponse.StatusCode);
    }
    
    [Fact]
    public virtual async Task GetPagedEntities_WhenPaginationQueryParametersIsNotValid_ReturnsStatusCode400()
    {
        var paginationQueryParameters = new PaginationQueryParameters { IndexFrom = 0, PageIndex = 0, PageSize = 5 };
        var entities = Fixture.CreateMany<TEntity>(10);
        var pagedCollection = new PagedCollection<TEntity>(entities, paginationQueryParameters);
        
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(x => 
                x.Send(It.IsAny<GetEntityPagedQuery<TEntity, TKey>>(), It.IsAny<CancellationToken>()))!
            .ReturnsAsync(pagedCollection);

        var controller = FakeControllerFactory.Create<UnitOfWorkReadOnlyController<TEntity, TKey>>(mediatorMock.Object);

        paginationQueryParameters.PageSize = -5;
        var actionResult = await controller.GetPagedEntitiesAsync(paginationQueryParameters);

        var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        var apiValidationFailureResponse = Assert.IsType<ApiValidationFailureResponse<ValidationFailure>>(badRequestObjectResult.Value);
        Assert.Equal(StatusCodes.Status400BadRequest, apiValidationFailureResponse.StatusCode);
    }
}