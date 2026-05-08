using System;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using BetTracker.Core.Models;
using BetTracker.Core.Services;

namespace BetTracker.UI.ViewModels;

/// <summary>
/// ViewModel representing a single bet item for display in a list.
/// Provides calculated properties and EV information.
/// </summary>
public partial class BetItemViewModel : ObservableObject
{
    private readonly IBetCalculationService _calculationService;

    [ObservableProperty]
    private Offre bet;

    [ObservableProperty]
    private decimal calculatedEV = 0;

    /// <summary>
    /// Constructor with bet and calculation service.
    /// </summary>
    public BetItemViewModel(Offre bet, IBetCalculationService calculationService)
    {
        _calculationService = calculationService ?? throw new ArgumentNullException(nameof(calculationService));
        Bet = bet ?? throw new ArgumentNullException(nameof(bet));

        CalculateEV();
    }

    /// <summary>
    /// Gets the bet type as a string.
    /// </summary>
    public string BetType => Bet.Type.ToString();

    /// <summary>
    /// Gets the bet status as a string.
    /// </summary>
    public string BetStatus => Bet.Statut.ToString();

    /// <summary>
    /// Gets the bookmaker name for display.
    /// </summary>
    public string BookmakerName => Bet.Bookmaker?.Nom ?? "Unknown";

    /// <summary>
    /// Calculates the EV based on the first calculation record.
    /// </summary>
    private void CalculateEV()
    {
        try
        {
            if (Bet.Calculs != null && Bet.Calculs.Count > 0)
            {
                var calcul = Bet.Calculs.FirstOrDefault();
                if (calcul != null)
                {
                    CalculatedEV = _calculationService.CalculateLayBetEV(
                        calcul.MiseBook1,
                        calcul.CoteBook1,
                        calcul.MiseBook2,
                        calcul.CoteBook2
                    );
                }
            }
        }
        catch (Exception)
        {
            // If calculation fails, EV remains 0
            CalculatedEV = 0;
        }
    }
}
