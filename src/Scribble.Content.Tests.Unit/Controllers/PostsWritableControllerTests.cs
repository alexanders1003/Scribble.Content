using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Scribble.Content.Infrastructure.UnitOfWork.Pagination;
using Scribble.Content.Models;
using Scribble.Content.Web.Controllers;
using Scribble.Content.Web.Features.Commands.Posts;
using Scribble.Content.Web.Features.Queries;
using Scribble.Content.Web.Features.Queries.Posts;
using Scribble.Content.Web.Models.Entities;
using Scribble.Responses;
using Xunit;

namespace Scribble.Content.Tests.Unit.Controllers;

public class PostsWritableControllerTests : UnitOfWorkWritableControllerTests<PostEntity, Guid, PostViewModel>
{
    [Fact]
    public async Task GetPostPagedByTagId_WhenPaginationQueryParametersIsValid_ReturnsStatusCode200()
    {
        var paginationQueryParameters = new PaginationQueryParameters { IndexFrom = 0, PageIndex = 0, PageSize = 5 };
        var entities = Fixture.CreateMany<PostEntity>(10);
        var pagedCollection = new PagedCollection<PostEntity>(entities, paginationQueryParameters);
        
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(
                x => x.Send(It.IsAny<GetEntityPagedQuery<PostEntity, Guid>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedCollection);

        var controller = FakeControllerFactory.Create<PostsWritableController>(mediatorMock.Object);

        var result = await controller.GetPostPagedByTagIdAsync(It.IsAny<Guid>(), paginationQueryParameters);

        var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
        var apiResultResponse = Assert.IsType<ApiResultResponse<IPagedCollection<PostEntity>>>(okObjectResult.Value);
        Assert.Equal(StatusCodes.Status200OK, apiResultResponse.StatusCode);
    }
    
    [Fact]
    public async Task GetPostPagedByTagName_WhenPaginationQueryParametersIsValid_ReturnsStatusCode200()
    {
        var paginationQueryParameters = new PaginationQueryParameters { IndexFrom = 0, PageIndex = 0, PageSize = 5 };
        var entities = Fixture.CreateMany<PostEntity>(10);
        var pagedCollection = new PagedCollection<PostEntity>(entities, paginationQueryParameters);
        
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(
                x => x.Send(It.IsAny<GetEntityPagedQuery<PostEntity, Guid>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedCollection);

        var controller = FakeControllerFactory.Create<PostsWritableController>(mediatorMock.Object);

        var result = await controller.GetPostPagedByTagNameAsync(It.IsAny<string>(), paginationQueryParameters);

        var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
        var apiResultResponse = Assert.IsType<ApiResultResponse<IPagedCollection<PostEntity>>>(okObjectResult.Value);
        Assert.Equal(StatusCodes.Status200OK, apiResultResponse.StatusCode);
    }


    [Fact]
    public async Task GetPendingPosts_ReturnsStatusCode200()
    {
        var entities = Fixture.CreateMany<PostEntity>();
        
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(
                x => x.Send(It.IsAny<GetPendingPostsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(entities);

        var controller = FakeControllerFactory.Create<PostsWritableController>(mediatorMock.Object);

        var result = await controller.GetAllPendingPostsAsync();

        var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
        var apiResultResponse = Assert.IsType<ApiResultResponse<IEnumerable<PostEntity>>>(okObjectResult.Value);
        Assert.Equal(StatusCodes.Status200OK, apiResultResponse.StatusCode);
    }

    [Fact]
    public async Task PublishPostAsync_ReturnsStatusCode200()
    {
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(
                x => x.Send(It.IsAny<PublishPostCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(MediatR.Unit.Value);

        var controller = FakeControllerFactory.Create<PostsWritableController>(mediatorMock.Object);

        var result = await controller.PublishPostAsync(It.IsAny<Guid>());

        var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
        var apiResponse = Assert.IsType<ApiResponse>(okObjectResult.Value);
        Assert.Equal(StatusCodes.Status204NoContent, apiResponse.StatusCode);
    }

    [Fact]
    public async Task GetViewsCount_ReturnsStatusCode200()
    {
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(
                x => x.Send(It.IsAny<GetViewsCountQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(It.IsAny<int>());

        var controller = FakeControllerFactory.Create<PostsWritableController>(mediatorMock.Object);

        var result = await controller.GetViewsCountAsync(It.IsAny<Guid>());

        var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
        var apiResultResponse = Assert.IsType<ApiResultResponse<int>>(okObjectResult.Value);
        Assert.Equal(StatusCodes.Status200OK, apiResultResponse.StatusCode);
    }
    
    [Fact]
    public async Task SetViewsCount_ReturnsStatusCode200()
    {
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(
                x => x.Send(It.IsAny<SetViewsCountCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(MediatR.Unit.Value);

        var controller = FakeControllerFactory.Create<PostsWritableController>(mediatorMock.Object);

        var result = await controller.SetViewsCountAsync(It.IsAny<Guid>());

        var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
        var apiResponse = Assert.IsType<ApiResponse>(okObjectResult.Value);
        Assert.Equal(StatusCodes.Status204NoContent, apiResponse.StatusCode);
    }
}