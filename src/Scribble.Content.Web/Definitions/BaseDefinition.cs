using AutoWrapper;
using Calabonga.AspNetCore.AppDefinitions;
using Microsoft.AspNetCore.Mvc;

namespace Scribble.Content.Web.Definitions;

public class BaseDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddControllers();

        services.AddResponseCaching();
        services.AddMemoryCache();
        
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });
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

        app.UseAutoWrapper(new AutoWrapperOptions
        {
            ShowApiVersion = true,
            ShowStatusCode = true,
            ShowIsErrorFlagForSuccessfulResponse = false,
            ShouldLogRequestData = false
        });
    }
}