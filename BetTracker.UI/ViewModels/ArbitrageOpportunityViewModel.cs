using System;
using CommunityToolkit.Mvvm.ComponentModel;
using BetTracker.Core.Models;

namespace BetTracker.UI.ViewModels;

/// <summary>
/// ViewModel wrapping an ArbitrageOpportunity model with display properties.
/// Provides formatted display strings for UI binding.
/// </summary>
public partial class ArbitrageOpportunityViewModel : ObservableObject
{
    [ObservableProperty]
    private ArbitrageOpportunity opportunity = null!;

    /// <summary>
    /// Display name for the first bookmaker.
    /// </summary>
    public string Bookmaker1 => $"Book #{Opportunity.Bookmaker1Id}";

    /// <summary>
    /// Display name for the second bookmaker.
    /// </summary>
    public string Bookmaker2 => $"Book #{Opportunity.Bookmaker2Id}";

    /// <summary>
    /// Formatted profit margin display with percentage sign.
    /// </summary>
    public string ProfitDisplay => $"+{Opportunity.ProfitMargin:F2}%";

    public ArbitrageOpportunityViewModel(ArbitrageOpportunity opportunity)
    {
        Opportunity = opportunity ?? throw new ArgumentNullException(nameof(opportunity));
    }
}
