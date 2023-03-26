using Calabonga.AspNetCore.AppDefinitions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Scribble.Content.Web.Definitions.Documentation.Conventions;
using Serilog;

namespace Scribble.Content.Web.Definitions;

public class DefinitionBase : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddControllers(options =>
        {
            options.Conventions.Add(new UnitOfWorkControllerConversion());
        });

        services.AddResponseCaching();
        services.AddMemoryCache();

        services.AddAutoMapper(typeof(Program).Assembly);
        
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });
        
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(dispose: true);
        });
        
        services.AddODataQueryFilter();
    }

    public override void ConfigureApplication(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseResponseCaching();
        app.UseHsts();
    }
}