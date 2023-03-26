using Calabonga.AspNetCore.AppDefinitions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Scribble.Content.Web.Definitions.Authorization;

public class AuthorizationDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, WebApplicationBuilder builder)
    {
        var config = builder.Configuration.GetSection("Identity")
            .Get<AuthorizationConfiguration>();

        if (config is null)
            throw new ArgumentNullException(nameof(config), 
                "Identity configuration section was not found in the appsettings.json file. Add this configuration and try again");
        
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.SaveToken = true;

                options.Authority = config.Authority;
                options.Audience = config.Audience;
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = config.Issuer,
                    ValidateIssuer = true,

                    ValidAudience = config.Audience,
                    ValidateAudience = true,

                    ValidateLifetime = true,
                };
            });
    }
    
    public override void ConfigureApplication(WebApplication app)
    {
        app.UseRouting();
        
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
    }
}