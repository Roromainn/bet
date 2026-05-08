using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BetTracker.Core.Models;
using BetTracker.Data.Repositories;

namespace BetTracker.UI.ViewModels;

/// <summary>
/// ViewModel for managing bookmakers and their offers.
/// Handles loading, adding, and updating bookmakers.
/// </summary>
public partial class BookmakerViewModel : ObservableObject
{
    private readonly IBookmakerRepository _bookmakerRepository;

    [ObservableProperty]
    private ObservableCollection<BookmakerItemViewModel> bookmakers = new();

    [ObservableProperty]
    private bool isLoading = false;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private string newBookmakerName = string.Empty;

    [ObservableProperty]
    private string newBookmakerLogoUrl = string.Empty;

    /// <summary>
    /// Constructor with dependency injection.
    /// </summary>
    public BookmakerViewModel(IBookmakerRepository bookmakerRepository)
    {
        _bookmakerRepository = bookmakerRepository ?? throw new ArgumentNullException(nameof(bookmakerRepository));
    }

    /// <summary>
    /// Loads all active bookmakers from the repository.
    /// </summary>
    [RelayCommand]
    public async Task LoadBookmakers()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;

            var activeBookmakers = await _bookmakerRepository.GetActiveAsync();
            var bookmakerItems = activeBookmakers
                .Select(bm => new BookmakerItemViewModel(bm))
                .ToList();

            Bookmakers = new ObservableCollection<BookmakerItemViewModel>(bookmakerItems);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading bookmakers: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Adds a new bookmaker.
    /// </summary>
    [RelayCommand]
    public async Task AddBookmaker()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;

            if (string.IsNullOrWhiteSpace(NewBookmakerName))
            {
                ErrorMessage = "Bookmaker name is required";
                return;
            }

            var newBookmaker = new Bookmaker
            {
                Nom = NewBookmakerName.Trim(),
                UrlLogo = string.IsNullOrWhiteSpace(NewBookmakerLogoUrl) ? string.Empty : NewBookmakerLogoUrl.Trim(),
                Actif = true,
                DateActivation = DateTime.Now
            };

            await _bookmakerRepository.AddAsync(newBookmaker);
            await _bookmakerRepository.SaveChangesAsync();

            // Add to local collection
            Bookmakers.Add(new BookmakerItemViewModel(newBookmaker));

            // Clear input fields
            NewBookmakerName = string.Empty;
            NewBookmakerLogoUrl = string.Empty;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error adding bookmaker: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Deletes a bookmaker by ID.
    /// </summary>
    [RelayCommand]
    public async Task DeleteBookmaker(int bookmakerId)
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;

            await _bookmakerRepository.DeleteAsync(bookmakerId);
            await _bookmakerRepository.SaveChangesAsync();

            // Remove from local collection
            var itemToRemove = Bookmakers.FirstOrDefault(b => b.Bookmaker.Id == bookmakerId);
            if (itemToRemove != null)
            {
                Bookmakers.Remove(itemToRemove);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error deleting bookmaker: {ex.Message}";
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
