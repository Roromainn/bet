using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BetTracker.Core.Services;

namespace BetTracker.UI.ViewModels;

/// <summary>
/// ViewModel for managing arbitrage opportunity discovery and alerts.
/// Handles finding opportunities and sending notifications.
/// </summary>
public partial class ArbitrageViewModel : ObservableObject
{
    private readonly IArbitrageFinderService _arbitrageFinderService;
    private readonly INotificationService _notificationService;

    [ObservableProperty]
    private ObservableCollection<ArbitrageOpportunityViewModel> opportunities = new();

    [ObservableProperty]
    private decimal minProfitThreshold = 5m;

    [ObservableProperty]
    private bool isLoading = false;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private ArbitrageOpportunityViewModel? selectedOpportunity;

    /// <summary>
    /// Constructor with dependency injection.
    /// </summary>
    public ArbitrageViewModel(
        IArbitrageFinderService arbitrageFinderService,
        INotificationService notificationService)
    {
        _arbitrageFinderService = arbitrageFinderService ?? throw new ArgumentNullException(nameof(arbitrageFinderService));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
    }

    /// <summary>
    /// Finds arbitrage opportunities based on the minimum profit threshold.
    /// </summary>
    [RelayCommand]
    public async Task FindOpportunities()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;

            var opportunities = await _arbitrageFinderService.FindArbitrageOpportunitiesAsync(MinProfitThreshold);
            var opportunityVMs = opportunities
                .Select(opp => new ArbitrageOpportunityViewModel(opp))
                .ToList();

            Opportunities = new ObservableCollection<ArbitrageOpportunityViewModel>(opportunityVMs);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error finding opportunities: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Sends an arbitrage alert for the selected opportunity.
    /// </summary>
    [RelayCommand]
    public async Task SendAlert()
    {
        if (SelectedOpportunity == null)
        {
            ErrorMessage = "Please select an opportunity first";
            return;
        }

        try
        {
            IsLoading = true;
            ErrorMessage = null;

            await _notificationService.SendArbitrageAlertAsync(
                SelectedOpportunity.Bookmaker1,
                SelectedOpportunity.Bookmaker2,
                SelectedOpportunity.Opportunity.ProfitMargin);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error sending alert: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Clears the current error message.
    /// </summary>
    [RelayCommand]
    public void ClearError()
    {
        ErrorMessage = null;
    }
}
