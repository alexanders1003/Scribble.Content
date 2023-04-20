using System.Reflection;
using Calabonga.AspNetCore.AppDefinitions;
using Microsoft.EntityFrameworkCore;
using Scribble.Content.Infrastructure.Contexts;
using Serilog;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = Program.CreateSerilogLogger(builder.Configuration);

try
{
    Log.Information($"Configuring web host {Program.ApplicationName}...");
    builder.Services.AddDefinitions(builder, typeof(Program));

    var app = builder.Build();

    app.UseDefinitions();

    Log.Information($"Applying migrations {Program.ApplicationName}...");
    using var scope = app.Services.CreateScope();

    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    db.Database.EnsureCreated();

    Log.Information($"Starting web host ({Program.ApplicationName})...");
    app.Run();
}
catch (Exception exp)
{
    Log.Fatal(exp, $"Web host terminated unexpectedly ({Program.ApplicationName})!");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program
{
    private static string? ApplicationName 
        => Assembly.GetExecutingAssembly().GetName().Name;

    private static ILogger CreateSerilogLogger(IConfiguration configuration)
    {
        return new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
    }
}