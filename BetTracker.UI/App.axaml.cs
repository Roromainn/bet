using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using System;
using Microsoft.Extensions.DependencyInjection;
using BetTracker.Data;
using BetTracker.Data.Repositories;
using BetTracker.Core.Services;
using BetTracker.UI.ViewModels;

namespace BetTracker.UI;

public partial class App : Application
{
    /// <summary>
    /// Global service provider for dependency injection.
    /// Initialized in OnFrameworkInitializationCompleted().
    /// </summary>
    public static IServiceProvider? ServiceProvider { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Build the DI container
        var services = new ServiceCollection();

        // Register DbContext with scoped lifetime
        services.AddScoped<BetTrackerContext>();

        // Register repositories and services from Phase 3
        services.AddBetTrackerRepositories();
        services.AddBetTrackerCoreServices();

        // Register ViewModels with transient lifetime (new instance per use)
        services.AddTransient<BetsViewModel>();
        services.AddTransient<BookmakerViewModel>();
        services.AddTransient<CalculationViewModel>();
        services.AddTransient<ArbitrageViewModel>();
        services.AddTransient<RiskDashboardViewModel>();

        // Build the service provider
        ServiceProvider = services.BuildServiceProvider();

        // Create main window
        var mainWindow = new MainWindow();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }
}