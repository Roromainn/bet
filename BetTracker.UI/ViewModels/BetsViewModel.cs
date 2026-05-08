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
            ErrorMessage = $"Error loading bets: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Deletes a bet by ID.
    /// </summary>
    [RelayCommand]
    public async Task DeleteBet(int betId)
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;

            await _betRepository.DeleteAsync(betId);
            await _betRepository.SaveChangesAsync();

            // Remove from local collection
            var itemToRemove = Bets.FirstOrDefault(b => b.Bet.Id == betId);
            if (itemToRemove != null)
            {
                Bets.Remove(itemToRemove);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error deleting bet: {ex.Message}";
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
