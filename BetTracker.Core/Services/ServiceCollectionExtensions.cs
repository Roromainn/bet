using Microsoft.Extensions.DependencyInjection;

namespace BetTracker.Core.Services;

/// <summary>
/// Extension methods for registering services in the DI container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register all core services in the dependency injection container.
    /// </summary>
    public static IServiceCollection AddBetTrackerCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IBetCalculationService, BetCalculationService>();
        return services;
    }
}
