using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BetTracker.Core.Models;
using BetTracker.Core.Services;
using BetTracker.Data.Repositories;

namespace BetTracker.UI.ViewModels;

/// <summary>
/// ViewModel for managing the list of bets.
/// Handles loading, filtering, and deleting bets.
/// </summary>
public partial class BetsViewModel : ObservableObject
{
    private readonly IBetRepository _betRepository;
    private readonly IBetCalculationService _calculationService;

    [ObservableProperty]
    private ObservableCollection<BetItemViewModel> bets = new();

    [ObservableProperty]
    private bool isLoading = false;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private bool showDeleteConfirm = false;

    [ObservableProperty]
    private string? confirmationMessage;

    [ObservableProperty]
    private BetItemViewModel? selectedBetForDelete;

    [ObservableProperty]
    private decimal newBetOdds = 2.0m;

    [ObservableProperty]
    private decimal newBetStake = 100m;

    [ObservableProperty]
    private string newBetType = string.Empty;

    [ObservableProperty]
    private string? oddsError;

    [ObservableProperty]
    private string? stakeError;

    [ObservableProperty]
    private string? betTypeError;

    /// <summary>
    /// Constructor with dependency injection.
    /// </summary>
    public BetsViewModel(
        IBetRepository betRepository,
        IBetCalculationService calculationService)
    {
        _betRepository = betRepository ?? throw new ArgumentNullException(nameof(betRepository));
        _calculationService = calculationService ?? throw new ArgumentNullException(nameof(calculationService));
    }

    /// <summary>
    /// Loads all active bets from the repository.
    /// </summary>
    [RelayCommand]
    public async Task LoadBets()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;

            var activeBets = await _betRepository.GetActiveBetsAsync();
            var betItems = activeBets
                .Select(bet => new BetItemViewModel(bet, _calculationService))
                .ToList();

            Bets = new ObservableCollection<BetItemViewModel>(betItems);
        }
        catch (Exception ex)
        {
            ErrorMessage = "Failed to load bets. Please try again.";
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Shows a confirmation dialog before deleting a bet.
    /// </summary>
    [RelayCommand]
    public void ConfirmDelete(BetItemViewModel bet)
    {
        SelectedBetForDelete = bet;
        ConfirmationMessage = $"Delete bet #{bet.Bet.Id}? This cannot be undone.";
        ShowDeleteConfirm = true;
    }

    /// <summary>
    /// Cancels the delete operation and hides the confirmation dialog.
    /// </summary>
    [RelayCommand]
    public void CancelDelete()
    {
        ShowDeleteConfirm = false;
        SelectedBetForDelete = null;
        ConfirmationMessage = null;
    }

    /// <summary>
    /// Executes the delete operation after confirmation.
    /// </summary>
    [RelayCommand]
    public async Task ExecuteDelete()
    {
        try
        {
            if (SelectedBetForDelete is not null)
            {
                IsLoading = true;
                ErrorMessage = null;

                await _betRepository.DeleteAsync(SelectedBetForDelete.Bet.Id);
                await _betRepository.SaveChangesAsync();

                Bets.Remove(SelectedBetForDelete);
                SelectedBetForDelete = null;
            }
            ShowDeleteConfirm = false;
        }
        catch (Exception ex)
        {
            ErrorMessage = "Failed to delete bet. Please try again.";
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

    /// <summary>
    /// Clears all filters and reloads all bets.
    /// </summary>
    [RelayCommand]
    public async Task ClearFilters()
    {
        IsLoading = true;
        ErrorMessage = null;
        try
        {
            Bets.Clear();
            await LoadBets();
        }
        catch (Exception ex)
        {
            ErrorMessage = "Failed to reload bets. Please try again.";
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Validates the bet input fields.
    /// </summary>
    private void ValidateFields()
    {
        OddsError = null;
        StakeError = null;
        BetTypeError = null;

        if (NewBetOdds <= 0)
        {
            OddsError = "Odds must be a positive number";
        }

        if (NewBetStake <= 0)
        {
            StakeError = "Stake must be a positive number";
        }

        if (string.IsNullOrWhiteSpace(NewBetType))
        {
            BetTypeError = "Bet type is required";
        }
    }

    /// <summary>
    /// Places a new bet.
    /// </summary>
    [RelayCommand]
    public void PlaceBet()
    {
        ErrorMessage = null;
        ValidateFields();

        if (OddsError != null || StakeError != null || BetTypeError != null)
        {
            return;
        }

        // Placeholder for actual bet placement logic
        ErrorMessage = "Bet placement feature is under development.";
    }
}
