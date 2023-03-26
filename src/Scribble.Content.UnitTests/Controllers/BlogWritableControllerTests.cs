using AutoFixture;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Scribble.Content.Infrastructure.UnitOfWork.Pagination;
using Scribble.Content.Models;
using Scribble.Content.Web.Controllers;
using Scribble.Content.Web.Features.Queries;
using Scribble.Content.Web.Models.Entities;
using Scribble.Responses;
using Xunit;

namespace Scribble.Content.UnitTests.Controllers;

public class BlogsWritableControllerTests : UnitOfWorkWritableControllerTests<BlogEntity, Guid, BlogViewModel>
{
    [Fact]
    public async Task GetBlogPagedByAuthorId_WhenPaginationQueryParametersIsValid_ReturnsStatusCode200()
    {
        var paginationQueryParameters = new PaginationQueryParameters { IndexFrom = 0, PageIndex = 0, PageSize = 5 };
        var entities = Fixture.CreateMany<BlogEntity>(10);
        var pagedCollection = new PagedCollection<BlogEntity>(entities, paginationQueryParameters);
        
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(
                x => x.Send(It.IsAny<GetEntityPagedQuery<BlogEntity, Guid>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedCollection);

        var controller = FakeControllerFactory.Create<BlogsWritableController>(mediatorMock.Object);

        var result = await controller.GetBlogPagedByAuthorIdAsync(It.IsAny<Guid>(), paginationQueryParameters);

        var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
        var apiResultResponse = Assert.IsType<ApiResultResponse<IPagedCollection<BlogEntity>>>(okObjectResult.Value);
        Assert.Equal(StatusCodes.Status200OK, apiResultResponse.StatusCode);
    }
    
    [Fact]
    public async Task GetBlogPagedByAuthorId_WhenPaginationQueryParametersIsNotValid_ReturnsStatusCode400()
    {
        var paginationQueryParameters = new PaginationQueryParameters { IndexFrom = 0, PageIndex = 0, PageSize = 5 };
        var entities = Fixture.CreateMany<BlogEntity>(10);
        var pagedCollection = new PagedCollection<BlogEntity>(entities, paginationQueryParameters);
        
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(
                x => x.Send(It.IsAny<GetEntityPagedQuery<BlogEntity, Guid>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedCollection);

        var controller = FakeControllerFactory.Create<BlogsWritableController>(mediatorMock.Object);

        paginationQueryParameters.PageSize = -1000;
        var result = await controller.GetBlogPagedByAuthorIdAsync(It.IsAny<Guid>(), paginationQueryParameters);

        var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        var apiValidationFailureResponse = Assert.IsType<ApiValidationFailureResponse<ValidationFailure>>(badRequestObjectResult.Value);
        Assert.Equal(StatusCodes.Status400BadRequest, apiValidationFailureResponse.StatusCode);
    }
}