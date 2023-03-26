using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Scribble.Content.IntegrationTests.Infrastructure;
using Scribble.Content.Models;
using Scribble.Responses;

namespace Scribble.Content.IntegrationTests.Controllers;

public class BlogsControllerIntegrationTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;

    public BlogsControllerIntegrationTests(TestWebApplicationFactory<Program> factory) 
        => _factory = factory;

    /*[Theory, AutoMoqFixture]
    public async Task CreateBlogAsync_WhenModelHasValidationErrors_ReturnsBadRequest(BlogEntity entity)
    {
        entity.BlogName = null!;

        var client = _factory.CreateClient();
        client.BaseAddress = new Uri("https://localhost:5003");

        var response = await client.PostAsJsonAsync("api/1.0/blogs", entity, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.Never
            })
            .ConfigureAwait(false);
        
        var content = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<ApiValidationFailureResponse<ValidationFailure>>(content);
        
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        Assert.NotNull(result.Errors);
        Assert.Equal(1, result.Errors?.Count());
    }*/

}