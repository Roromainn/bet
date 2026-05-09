using BetTracker.Core.Models;

namespace BetTracker.Core.Services;

/// <summary>
/// Repository abstraction for Offre (Bet) entities.
/// Used by services in Core layer without coupling to Data layer.
/// </summary>
public interface IBetOfferRepository
{
    /// <summary>
    /// Get all active bets (offers).
    /// </summary>
    Task<IEnumerable<Offre>> GetActiveBetsAsync();
}
