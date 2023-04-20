using Calabonga.AspNetCore.AppDefinitions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Scribble.Content.Infrastructure.Contexts;
using Scribble.Content.Infrastructure.UnitOfWork.Extensions;
using Scribble.Content.Web.Models.Validators.Extensions;

namespace Scribble.Content.Web.Definitions.Infrastructure;

public class InfrastructureDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
        });

        services.AddUnitOfWork<ApplicationDbContext>();

        services.AddValidatorsFromAssembly(typeof(Program).Assembly);
        services.AddViewModelValidators();
    }

    public override void ConfigureApplication(WebApplication app)
    {
        var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        
        var context = services.GetRequiredService<ApplicationDbContext>();

        context.Database.EnsureCreatedAsync().Wait();
    }
}