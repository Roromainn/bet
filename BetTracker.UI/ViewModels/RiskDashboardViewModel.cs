using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BetTracker.Core.Services;

namespace BetTracker.UI.ViewModels;

/// <summary>
/// ViewModel for risk assessment and dashboard calculations.
/// Provides real-time risk assessment of back and lay bet configurations.
/// </summary>
public partial class RiskDashboardViewModel : ObservableObject
{
    private readonly IRiskAssessmentService _riskAssessmentService;
    private readonly IProfitCalculatorService _profitCalculatorService;

    // Input properties (observable, user-editable)
    [ObservableProperty]
    private decimal backStake = 100m;

    [ObservableProperty]
    private decimal backOdds = 3.0m;

    [ObservableProperty]
    private decimal layStake = 150m;

    [ObservableProperty]
    private decimal layOdds = 1.5m;

    // Output properties (read-only, calculated)
    [ObservableProperty]
    private decimal netProfit = 0m;

    [ObservableProperty]
    private decimal liability = 0m;

    [ObservableProperty]
    private string riskLevelDisplay = "N/A";

    [ObservableProperty]
    private decimal maxExposure = 0m;

    [ObservableProperty]
    private string? errorMessage;

    /// <summary>
    /// Constructor with dependency injection.
    /// </summary>
    public RiskDashboardViewModel(
        IRiskAssessmentService riskAssessmentService,
        IProfitCalculatorService profitCalculatorService)
    {
        _riskAssessmentService = riskAssessmentService ?? throw new ArgumentNullException(nameof(riskAssessmentService));
        _profitCalculatorService = profitCalculatorService ?? throw new ArgumentNullException(nameof(profitCalculatorService));
    }

    /// <summary>
    /// Assesses the risk of the current bet configuration.
    /// Calculates NetProfit, Liability, RiskLevel, and MaxExposure.
    /// </summary>
    [RelayCommand]
    public void Assess()
    {
        try
        {
            ErrorMessage = null;

            // Calculate liability from lay stake and odds
            Liability = _profitCalculatorService.CalculateLiability(LayStake, LayOdds);

            // Calculate net profit
            NetProfit = _profitCalculatorService.CalculateNetProfit(BackStake, BackOdds, LayStake, LayOdds);

            // Assess risk level and convert enum to string
            var riskLevel = _riskAssessmentService.AssessRisk(BackStake, Liability);
            RiskLevelDisplay = riskLevel.ToString();

            // Get maximum safe exposure
            MaxExposure = _riskAssessmentService.GetMaximumExposure(Liability);
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
    /// Clears all values to defaults and resets error message.
    /// </summary>
    [RelayCommand]
    public void Clear()
    {
        BackStake = 100m;
        BackOdds = 3.0m;
        LayStake = 150m;
        LayOdds = 1.5m;

        NetProfit = 0m;
        Liability = 0m;
        RiskLevelDisplay = "N/A";
        MaxExposure = 0m;

        ErrorMessage = null;
    }
}
