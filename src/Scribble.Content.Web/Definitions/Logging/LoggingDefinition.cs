using Calabonga.AspNetCore.AppDefinitions;
using Serilog;

namespace Scribble.Content.Web.Definitions.Logging;

public class LoggingDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(dispose: true);
        });
    }
}