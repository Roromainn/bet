using Microsoft.Extensions.DependencyInjection;

namespace BetTracker.Data.Repositories;

/// <summary>
/// Extension methods for registering repositories in the DI container.
/// </summary>
public static class RepositoryServiceCollectionExtensions
{
    /// <summary>
    /// Register all repositories in the dependency injection container.
    /// </summary>
    public static IServiceCollection AddBetTrackerRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IBookmakerRepository, BookmakerRepository>();
        services.AddScoped<IBetRepository, BetRepository>();

        return services;
    }
}
