using Calabonga.AspNetCore.AppDefinitions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Scribble.Content.Infrastructure.Contexts;
using Scribble.Content.Infrastructure.UnitOfWork.Extensions;

namespace Scribble.Content.Web.Definitions.Data;

public class DataDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
        });

        services.AddUnitOfWork<ApplicationDbContext>();

        services.AddValidatorsFromAssembly(typeof(Program).Assembly);
    }
}