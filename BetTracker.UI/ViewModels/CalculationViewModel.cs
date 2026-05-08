using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BetTracker.Core.Services;

namespace BetTracker.UI.ViewModels;

/// <summary>
/// ViewModel for bet calculation operations.
/// Provides synchronous calculation methods for EV, matched bets, and coverage loss.
/// No async operations - all calculations are instant.
/// </summary>
public partial class CalculationViewModel : ObservableObject
{
    private readonly IBetCalculationService _calculationService;

    // Lay Bet EV Calculation properties
    [ObservableProperty]
    private decimal backStake = 100m;

    [ObservableProperty]
    private decimal backOdds = 3.0m;

    [ObservableProperty]
    private decimal layStake = 150m;

    [ObservableProperty]
    private decimal layOdds = 1.5m;

    [ObservableProperty]
    private decimal layBetEV = 0m;

    // Matched Bet Profit properties
    [ObservableProperty]
    private decimal stake1 = 100m;

    [ObservableProperty]
    private decimal odds1 = 2.0m;

    [ObservableProperty]
    private decimal stake2 = 100m;

    [ObservableProperty]
    private decimal odds2 = 2.0m;

    [ObservableProperty]
    private decimal matchedBetProfit = 0m;

    // Coverage Loss properties
    [ObservableProperty]
    private decimal coverageStake1 = 100m;

    [ObservableProperty]
    private decimal coverageOdds1 = 3.0m;

    [ObservableProperty]
    private decimal coverageStake2 = 100m;

    [ObservableProperty]
    private decimal coverageOdds2 = 1.5m;

    [ObservableProperty]
    private decimal coverageLoss = 0m;

    [ObservableProperty]
    private string? errorMessage;

    /// <summary>
    /// Constructor with dependency injection.
    /// </summary>
    public CalculationViewModel(IBetCalculationService calculationService)
    {
        _calculationService = calculationService ?? throw new ArgumentNullException(nameof(calculationService));
    }

    /// <summary>
    /// Calculates the EV for a lay bet based on current property values.
    /// Formula: EV = back_stake * (back_odds - 1) - lay_stake * lay_odds
    /// </summary>
    [RelayCommand]
    public void CalculateLayBetEV()
    {
        try
        {
            ErrorMessage = null;

            LayBetEV = _calculationService.CalculateLayBetEV(
                BackStake,
                BackOdds,
                LayStake,
                LayOdds
            );
        }
        catch (ArgumentException ex)
        {
            ErrorMessage = $"Calculation error: {ex.Message}";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Unexpected error: {ex.Message}";
        }
    }

    /// <summary>
    /// Calculates the guaranteed profit for matched bets.
    /// </summary>
    [RelayCommand]
    public void CalculateMatchedBetProfit()
    {
        try
        {
            ErrorMessage = null;

            MatchedBetProfit = _calculationService.CalculateMatchedBetProfit(
                Stake1,
                Odds1,
                Stake2,
                Odds2
            );
        }
        catch (ArgumentException ex)
        {
            ErrorMessage = $"Calculation error: {ex.Message}";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Unexpected error: {ex.Message}";
        }
    }

    /// <summary>
    /// Calculates the coverage loss due to imperfect matching.
    /// </summary>
    [RelayCommand]
    public void CalculateCoverageLoss()
    {
        try
        {
            ErrorMessage = null;

            CoverageLoss = _calculationService.CalculateCoverageLoss(
                CoverageStake1,
                CoverageOdds1,
                CoverageStake2,
                CoverageOdds2
            );
        }
        catch (ArgumentException ex)
        {
            ErrorMessage = $"Calculation error: {ex.Message}";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Unexpected error: {ex.Message}";
        }
    }

    /// <summary>
    /// Clears all calculations and resets to default values.
    /// </summary>
    [RelayCommand]
    public void Clear()
    {
        BackStake = 100m;
        BackOdds = 3.0m;
        LayStake = 150m;
        LayOdds = 1.5m;
        LayBetEV = 0m;

        Stake1 = 100m;
        Odds1 = 2.0m;
        Stake2 = 100m;
        Odds2 = 2.0m;
        MatchedBetProfit = 0m;

        CoverageStake1 = 100m;
        CoverageOdds1 = 3.0m;
        CoverageStake2 = 100m;
        CoverageOdds2 = 1.5m;
        CoverageLoss = 0m;

        ErrorMessage = null;
    }
}
