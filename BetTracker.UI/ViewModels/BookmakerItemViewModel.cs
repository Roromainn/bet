using System;
using CommunityToolkit.Mvvm.ComponentModel;
using BetTracker.Core.Models;

namespace BetTracker.UI.ViewModels;

/// <summary>
/// ViewModel representing a single bookmaker item for display in a list.
/// </summary>
public partial class BookmakerItemViewModel : ObservableObject
{
    [ObservableProperty]
    private Bookmaker bookmaker;

    /// <summary>
    /// Constructor with bookmaker.
    /// </summary>
    public BookmakerItemViewModel(Bookmaker bookmaker)
    {
        Bookmaker = bookmaker ?? throw new ArgumentNullException(nameof(bookmaker));
    }

    /// <summary>
    /// Gets the bookmaker name for display.
    /// </summary>
    public string Name => Bookmaker.Nom;

    /// <summary>
    /// Gets the bookmaker logo URL or a default message.
    /// </summary>
    public string LogoUrl => string.IsNullOrWhiteSpace(Bookmaker.UrlLogo) ? "No logo URL provided" : Bookmaker.UrlLogo;

    /// <summary>
    /// Gets the active status as a string.
    /// </summary>
    public string Status => Bookmaker.Actif ? "Active" : "Inactive";

    /// <summary>
    /// Gets a display-friendly string for the bookmaker.
    /// </summary>
    public override string ToString()
    {
        return $"{Name} ({Status})";
    }
}
