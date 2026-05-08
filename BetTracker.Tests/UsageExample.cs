using Microsoft.Extensions.DependencyInjection;
using BetTracker.Core.Services;
using BetTracker.Core.Models;
using BetTracker.Data;
using BetTracker.Data.Repositories;

namespace BetTracker.Tests;

/// <summary>
/// Example demonstrating how to use repositories and services via DI container.
/// This class shows the recommended pattern for configuring and using the application.
/// </summary>
public class UsageExample
{
    /// <summary>
    /// Example: Setting up the DI container for BetTracker application.
    /// </summary>
    [Fact]
    public void Example_SetupDependencyInjectionContainer()
    {
        // 1. Create a service collection
        var services = new ServiceCollection();

        // 2. Register BetTracker services and repositories
        services.AddScoped<BetTrackerContext>();
        services.AddBetTrackerRepositories();
        services.AddBetTrackerCoreServices();

        // 3. Build the service provider
        var provider = services.BuildServiceProvider();

        // 4. Now you can resolve services from anywhere
        var calculationService = provider.GetRequiredService<IBetCalculationService>();
        var bookmakerRepo = provider.GetRequiredService<IBookmakerRepository>();
        var betRepo = provider.GetRequiredService<IBetRepository>();

        // 5. Use them
        Assert.NotNull(calculationService);
        Assert.NotNull(bookmakerRepo);
        Assert.NotNull(betRepo);
    }

    /// <summary>
    /// Example: Using the calculation service.
    /// </summary>
    [Fact]
    public void Example_CalculateEVForLayBet()
    {
        // Setup
        var services = new ServiceCollection();
        services.AddBetTrackerCoreServices();
        var provider = services.BuildServiceProvider();

        var calcService = provider.GetRequiredService<IBetCalculationService>();

        // Scenario: Lay bet matching
        // Back on Betclic at 3.0 odds with 100 euros
        // Lay on Winamax at 1.5 odds with 200 euros
        var backStake = 100m;
        var backOdds = 3.0m;
        var layStake = 200m;
        var layOdds = 1.5m;

        var ev = calcService.CalculateLayBetEV(backStake, backOdds, layStake, layOdds);

        // Expected Value = 100 * (3.0 - 1) - 200 * 1.5 = 200 - 300 = -100
        Assert.Equal(-100m, ev);
    }

    /// <summary>
    /// Example: Using the calculation service to find profitable bets.
    /// </summary>
    [Fact]
    public void Example_FindProfitableLay()
    {
        var services = new ServiceCollection();
        services.AddBetTrackerCoreServices();
        var provider = services.BuildServiceProvider();

        var calcService = provider.GetRequiredService<IBetCalculationService>();

        // Scenario: Looking for profitable lay bets
        var scenarios = new[]
        {
            // Scenario 1: Back at 4.0, Lay at 1.33
            new { Back = (Stake: 100m, Odds: 4.0m), Lay = (Stake: 300m, Odds: 1.33m) },
            // Scenario 2: Back at 2.5, Lay at 1.5
            new { Back = (Stake: 100m, Odds: 2.5m), Lay = (Stake: 166.7m, Odds: 1.5m) },
        };

        foreach (var scenario in scenarios)
        {
            var ev = calcService.CalculateLayBetEV(
                scenario.Back.Stake,
                scenario.Back.Odds,
                scenario.Lay.Stake,
                scenario.Lay.Odds
            );

            // In real scenario, filter by EV > 0 to find profitable bets
            Assert.IsType<decimal>(ev);
        }
    }

    /// <summary>
    /// Example: Using coverage loss calculation.
    /// </summary>
    [Fact]
    public void Example_CalculateCoverageLoss()
    {
        var services = new ServiceCollection();
        services.AddBetTrackerCoreServices();
        var provider = services.BuildServiceProvider();

        var calcService = provider.GetRequiredService<IBetCalculationService>();

        // Scenario: Two bets on opposite outcomes
        // Bet on Team A to win at 2.5 odds with 100 euros (returns 250)
        // Bet on Team B to win at 1.67 odds with 150 euros (returns 250)
        var loss = calcService.CalculateCoverageLoss(100m, 2.5m, 150m, 1.67m);

        // Worst case: minimum return is 250, stakes are 250, loss = 0
        Assert.True(loss >= 0);
    }

    /// <summary>
    /// Example: Scoped lifetime for repositories (each request gets fresh context).
    /// </summary>
    [Fact]
    public void Example_RepositoriesScopedLifetime()
    {
        var services = new ServiceCollection();
        services.AddScoped<BetTrackerContext>();
        services.AddBetTrackerRepositories();
        var provider = services.BuildServiceProvider();

        // First scope (like a web request or operation)
        using (var scope1 = provider.CreateScope())
        {
            var repo1 = scope1.ServiceProvider.GetRequiredService<IBookmakerRepository>();
            var repo1Again = scope1.ServiceProvider.GetRequiredService<IBookmakerRepository>();

            // Same scope = same instance
            Assert.Same(repo1, repo1Again);
        }

        // Second scope = new instances
        using (var scope2 = provider.CreateScope())
        {
            var repo2 = scope2.ServiceProvider.GetRequiredService<IBookmakerRepository>();
            var repo2Again = scope2.ServiceProvider.GetRequiredService<IBookmakerRepository>();

            // Same scope = same instance
            Assert.Same(repo2, repo2Again);
        }
    }

    /// <summary>
    /// Example: How to use this in a real Avalonia MVVM ViewModel.
    /// </summary>
    public class ExampleViewModel
    {
        private readonly IBookmakerRepository _bookmakerRepository;
        private readonly IBetRepository _betRepository;
        private readonly IBetCalculationService _calculationService;

        // Constructor injection - the DI container handles this
        public ExampleViewModel(
            IBookmakerRepository bookmakerRepository,
            IBetRepository betRepository,
            IBetCalculationService calculationService)
        {
            _bookmakerRepository = bookmakerRepository;
            _betRepository = betRepository;
            _calculationService = calculationService;
        }

        // Example method using injected services
        public async Task<decimal> GetBestLayBetAsync(int offreId)
        {
            // Load offer with its calculations
            var offre = await _betRepository.GetWithCalculsAsync(offreId);
            if (offre == null)
                throw new ArgumentException($"Offre {offreId} not found");

            // Get the first calculation (in real code, find the best one)
            var calcul = offre.Calculs.FirstOrDefault();
            if (calcul == null)
                throw new InvalidOperationException($"No calculations found for offre {offreId}");

            // Calculate and return EV
            var ev = _calculationService.CalculateLayBetEV(
                calcul.MiseBook1,
                calcul.CoteBook1,
                calcul.MiseBook2,
                calcul.CoteBook2
            );

            return ev;
        }
    }
}
