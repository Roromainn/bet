using BetTracker.Core.Models;

namespace BetTracker.Core.Services;

/// <summary>
/// Implementation of arbitrage finder service for identifying profitable opportunities between bookmakers.
/// </summary>
public class ArbitrageFinderService : IArbitrageFinderService
{
    private readonly IBetOfferRepository _betRepository;
    private readonly IBetCalculationService _calculationService;

    /// <summary>
    /// Initialize ArbitrageFinderService with required dependencies.
    /// </summary>
    public ArbitrageFinderService(
        IBetOfferRepository betRepository,
        IBetCalculationService calculationService)
    {
        _betRepository = betRepository ?? throw new ArgumentNullException(nameof(betRepository));
        _calculationService = calculationService ?? throw new ArgumentNullException(nameof(calculationService));
    }

    /// <summary>
    /// Find arbitrage opportunities based on a minimum profit threshold.
    /// Groups bets by type and compares pairs from different bookmakers to identify profitable arbitrage situations.
    /// </summary>
    public async Task<IEnumerable<ArbitrageOpportunity>> FindArbitrageOpportunitiesAsync(decimal minProfitThreshold)
    {
        var activeBets = await _betRepository.GetActiveBetsAsync();
        var opportunities = new List<ArbitrageOpportunity>();

        // Group by bet type to find arbitrage pairs
        var betsByType = activeBets.GroupBy(b => b.Type).ToList();

        foreach (var group in betsByType)
        {
            var bets = group.ToList();
            for (int i = 0; i < bets.Count; i++)
            {
                for (int j = i + 1; j < bets.Count; j++)
                {
                    var bet1 = bets[i];
                    var bet2 = bets[j];

                    // Skip if from same bookmaker
                    if (bet1.BookmakerId == bet2.BookmakerId) continue;

                    // Calculate profit margin using lay bet EV calculation
                    var ev = _calculationService.CalculateLayBetEV(
                        bet1.MontantBonus, bet1.CoteMinimum,
                        bet2.MontantBonus, bet2.CoteMinimum
                    );

                    // Add if meets profit threshold
                    if (ev >= minProfitThreshold)
                    {
                        opportunities.Add(new ArbitrageOpportunity
                        {
                            Bookmaker1Id = bet1.BookmakerId,
                            Bookmaker2Id = bet2.BookmakerId,
                            ProfitMargin = ev,
                            CreatedAt = DateTime.Now
                        });
                    }
                }
            }
        }

        return opportunities;
    }
}
