using Calabonga.AspNetCore.AppDefinitions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace Scribble.Content.Web.Definitions.Documentation;

public class SwaggerDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddEndpointsApiExplorer();

        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
        });

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Scribble.Content",
                Version = ApiVersion.Default.ToString(),
            });
        });
    }
    
    public override void ConfigureApplication(WebApplication app)
    {
        app.UseStaticFiles();
        app.UseSwagger();
        app.UseSwaggerUI(options => {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger Configuration V1");
        });
    }
}