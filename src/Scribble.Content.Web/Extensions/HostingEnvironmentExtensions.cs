using System.Collections.ObjectModel;

namespace Scribble.Content.Web.Extensions;

public static class HostingEnvironmentExtensions
{
    public static bool IsTesting(this IWebHostEnvironment environment)
    {
        return environment.IsEnvironment(HostingEnvironmentDefaults.TestingEnvironment);
    }
}

public static class HostingEnvironmentDefaults
{
    public const string TestingEnvironment = "Testing";
}