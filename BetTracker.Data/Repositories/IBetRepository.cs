using BetTracker.Core.Models;

namespace BetTracker.Data.Repositories;

/// <summary>
/// Repository interface for Offre (Bet) entity with specialized queries.
/// </summary>
public interface IBetRepository : IRepository<Offre>
{
    /// <summary>
    /// Get all active bets (offers).
    /// </summary>
    Task<IEnumerable<Offre>> GetActiveBetsAsync();

    /// <summary>
    /// Get bets by bookmaker ID.
    /// </summary>
    Task<IEnumerable<Offre>> GetByBookmakerAsync(int bookmakerId);

    /// <summary>
    /// Get a bet with its calculations.
    /// </summary>
    Task<Offre?> GetWithCalculsAsync(int id);

    /// <summary>
    /// Get bets by type.
    /// </summary>
    Task<IEnumerable<Offre>> GetByTypeAsync(TypeOffre type);

    /// <summary>
    /// Get completed bets (offers).
    /// </summary>
    Task<IEnumerable<Offre>> GetCompletedBetsAsync();
}
