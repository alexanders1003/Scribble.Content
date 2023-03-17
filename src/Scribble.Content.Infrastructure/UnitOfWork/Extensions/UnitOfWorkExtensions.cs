using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scribble.Content.Infrastructure.UnitOfWork.Factories;

namespace Scribble.Content.Infrastructure.UnitOfWork.Extensions;

public static class UnitOfWorkExtensions
{
    public static IServiceCollection AddUnitOfWork<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext
    {
        services.AddScoped<IEntityRepositoryFactory, UnitOfWork<TDbContext>>();
        services.AddScoped<IUnitOfWork, UnitOfWork<TDbContext>>();
        services.AddScoped<IUnitOfWork<TDbContext>, UnitOfWork<TDbContext>>();

        return services;
    }
}