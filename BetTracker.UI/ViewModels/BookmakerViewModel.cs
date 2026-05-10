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

    [ObservableProperty]
    private bool showDeleteConfirm = false;

    [ObservableProperty]
    private string? confirmationMessage;

    [ObservableProperty]
    private BookmakerItemViewModel? selectedBookmakerForDelete;

    [ObservableProperty]
    private string? nameError;

    [ObservableProperty]
    private string? logoUrlError;

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
            ErrorMessage = "Failed to load bookmakers. Please try again.";
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
            ErrorMessage = null;
            ValidateFields();

            if (NameError != null || LogoUrlError != null)
            {
                return;
            }

            IsLoading = true;

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
            ErrorMessage = "Failed to add bookmaker. Please check your input and try again.";
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Shows a confirmation dialog before deleting a bookmaker.
    /// </summary>
    [RelayCommand]
    public void ConfirmDelete(BookmakerItemViewModel bookmaker)
    {
        SelectedBookmakerForDelete = bookmaker;
        ConfirmationMessage = $"Delete bookmaker '{bookmaker.Name}'? This cannot be undone.";
        ShowDeleteConfirm = true;
    }

    /// <summary>
    /// Cancels the delete operation and hides the confirmation dialog.
    /// </summary>
    [RelayCommand]
    public void CancelDelete()
    {
        ShowDeleteConfirm = false;
        SelectedBookmakerForDelete = null;
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
            if (SelectedBookmakerForDelete is not null)
            {
                IsLoading = true;
                ErrorMessage = null;

                await _bookmakerRepository.DeleteAsync(SelectedBookmakerForDelete.Bookmaker.Id);
                await _bookmakerRepository.SaveChangesAsync();

                Bookmakers.Remove(SelectedBookmakerForDelete);
                SelectedBookmakerForDelete = null;
            }
            ShowDeleteConfirm = false;
        }
        catch (Exception ex)
        {
            ErrorMessage = "Failed to delete bookmaker. Please try again.";
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
    /// Clears all filters and input fields, then reloads all bookmakers.
    /// </summary>
    [RelayCommand]
    public async Task ClearFilters()
    {
        IsLoading = true;
        ErrorMessage = null;
        try
        {
            Bookmakers.Clear();
            NewBookmakerName = string.Empty;
            NewBookmakerLogoUrl = string.Empty;
            await LoadBookmakers();
        }
        catch (Exception ex)
        {
            ErrorMessage = "Failed to reload bookmakers. Please try again.";
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Validates the bookmaker input fields.
    /// </summary>
    private void ValidateFields()
    {
        NameError = null;
        LogoUrlError = null;

        if (string.IsNullOrWhiteSpace(NewBookmakerName))
        {
            NameError = "Bookmaker name is required";
        }

        if (!string.IsNullOrWhiteSpace(NewBookmakerLogoUrl))
        {
            if (!Uri.TryCreate(NewBookmakerLogoUrl.Trim(), UriKind.Absolute, out _))
            {
                LogoUrlError = "Logo URL must be a valid URL";
            }
        }
    }
}
