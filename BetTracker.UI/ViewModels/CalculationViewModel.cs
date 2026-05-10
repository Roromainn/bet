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

    [ObservableProperty]
    private string? backStakeError;

    [ObservableProperty]
    private string? backOddsError;

    [ObservableProperty]
    private string? layStakeError;

    [ObservableProperty]
    private string? layOddsError;

    [ObservableProperty]
    private string? stake1Error;

    [ObservableProperty]
    private string? odds1Error;

    [ObservableProperty]
    private string? stake2Error;

    [ObservableProperty]
    private string? odds2Error;

    [ObservableProperty]
    private string? coverageStake1Error;

    [ObservableProperty]
    private string? coverageOdds1Error;

    [ObservableProperty]
    private string? coverageStake2Error;

    [ObservableProperty]
    private string? coverageOdds2Error;

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
            ValidateLayBetFields();

            if (BackStakeError != null || BackOddsError != null || LayStakeError != null || LayOddsError != null)
            {
                ErrorMessage = BackStakeError ?? BackOddsError ?? LayStakeError ?? LayOddsError;
                return;
            }

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
            ValidateMatchedBetFields();

            if (Stake1Error != null || Odds1Error != null || Stake2Error != null || Odds2Error != null)
            {
                ErrorMessage = Stake1Error ?? Odds1Error ?? Stake2Error ?? Odds2Error;
                return;
            }

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
            ValidateCoverageFields();

            if (CoverageStake1Error != null || CoverageOdds1Error != null || CoverageStake2Error != null || CoverageOdds2Error != null)
            {
                ErrorMessage = CoverageStake1Error ?? CoverageOdds1Error ?? CoverageStake2Error ?? CoverageOdds2Error;
                return;
            }

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
    public void ResetForm()
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
        ClearErrors();
    }

    /// <summary>
    /// Clears all validation error messages.
    /// </summary>
    private void ClearErrors()
    {
        BackStakeError = null;
        BackOddsError = null;
        LayStakeError = null;
        LayOddsError = null;
        Stake1Error = null;
        Odds1Error = null;
        Stake2Error = null;
        Odds2Error = null;
        CoverageStake1Error = null;
        CoverageOdds1Error = null;
        CoverageStake2Error = null;
        CoverageOdds2Error = null;
    }

    /// <summary>
    /// Validates lay bet EV calculation fields.
    /// </summary>
    private void ValidateLayBetFields()
    {
        BackStakeError = null;
        BackOddsError = null;
        LayStakeError = null;
        LayOddsError = null;

        if (BackStake <= 0)
        {
            BackStakeError = "Back stake must be a positive number";
        }

        if (BackOdds <= 0)
        {
            BackOddsError = "Back odds must be a positive number";
        }

        if (LayStake <= 0)
        {
            LayStakeError = "Lay stake must be a positive number";
        }

        if (LayOdds <= 0)
        {
            LayOddsError = "Lay odds must be a positive number";
        }
    }

    /// <summary>
    /// Validates matched bet profit calculation fields.
    /// </summary>
    private void ValidateMatchedBetFields()
    {
        Stake1Error = null;
        Odds1Error = null;
        Stake2Error = null;
        Odds2Error = null;

        if (Stake1 <= 0)
        {
            Stake1Error = "Stake 1 must be a positive number";
        }

        if (Odds1 <= 0)
        {
            Odds1Error = "Odds 1 must be a positive number";
        }

        if (Stake2 <= 0)
        {
            Stake2Error = "Stake 2 must be a positive number";
        }

        if (Odds2 <= 0)
        {
            Odds2Error = "Odds 2 must be a positive number";
        }
    }

    /// <summary>
    /// Validates coverage loss calculation fields.
    /// </summary>
    private void ValidateCoverageFields()
    {
        CoverageStake1Error = null;
        CoverageOdds1Error = null;
        CoverageStake2Error = null;
        CoverageOdds2Error = null;

        if (CoverageStake1 <= 0)
        {
            CoverageStake1Error = "Stake 1 must be a positive number";
        }

        if (CoverageOdds1 <= 0)
        {
            CoverageOdds1Error = "Odds 1 must be a positive number";
        }

        if (CoverageStake2 <= 0)
        {
            CoverageStake2Error = "Stake 2 must be a positive number";
        }

        if (CoverageOdds2 <= 0)
        {
            CoverageOdds2Error = "Odds 2 must be a positive number";
        }
    }
}
