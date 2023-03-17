using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using AutoWrapper.Models;
using AutoWrapper.Models.ResponseTypes;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Scribble.Content.Models;
using Scribble.Content.Tests.Integration.Infrastructure;

namespace Scribble.Content.Tests.Integration.Controllers;

public class BlogControllerIntegrationTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;

    public BlogControllerIntegrationTests(TestWebApplicationFactory<Program> factory) 
        => _factory = factory;

    [Theory, AutoMoqFixture]
    public async Task CreateBlogAsync_WhenModelIsValid_ReturnsStatusCode200(BlogEntity entity)
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsync("api/1.0/blogs", JsonContent.Create(entity));

        response.EnsureSuccessStatusCode();
        
        var content = await response.Content
            .ReadAsStringAsync().ConfigureAwait(false);

        var apiResponse = JsonConvert.DeserializeObject<ApiResultResponse<BlogEntity>>(content);
    }
}