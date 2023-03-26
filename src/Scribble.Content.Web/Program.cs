using Calabonga.AspNetCore.AppDefinitions;
using Scribble.Content.Infrastructure.Contexts;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
    
services.AddDefinitions(builder, typeof(Program));

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

var app = builder.Build();

app.UseDefinitions();

try
{
    Log.Information("Starting web host");
    foreach (var url in app.Urls)
    {
        Log.Information($"Now listening {url}");
    }

    using var scope = app.Services.CreateScope();

    var container = scope.ServiceProvider;
    var db = container.GetRequiredService<ApplicationDbContext>();

    db.Database.EnsureCreated();
    
    app.Run();
}
catch (Exception exp)
{
    Log.Fatal(exp, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }