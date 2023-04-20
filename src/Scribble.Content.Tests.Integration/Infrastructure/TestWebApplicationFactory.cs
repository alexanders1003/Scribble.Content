using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scribble.Content.Infrastructure.Contexts;
using Scribble.Content.Web.Extensions;

namespace Scribble.Content.Tests.Integration.Infrastructure;

// ReSharper disable once ClassNeverInstantiated.Global
public class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(HostingEnvironmentDefaults.TestingEnvironment);

        builder.UseUrls("https://localhost:5001");
        
        builder.ConfigureServices(services =>
        {
            services.AddSingleton<IAuthenticationService, TestAuthenticationService>();
            
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (dbContextDescriptor is not null)
                services.Remove(dbContextDescriptor);

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("Scribble.Content.Tests.Unit.Integration.InMemory");
            });
        });
    }
}