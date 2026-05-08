using BetTracker.Core.Models;

namespace BetTracker.Data.Repositories;

/// <summary>
/// Repository interface for Bookmaker entity with specialized queries.
/// </summary>
public interface IBookmakerRepository : IRepository<Bookmaker>
{
    /// <summary>
    /// Get all active bookmakers.
    /// </summary>
    Task<IEnumerable<Bookmaker>> GetActiveAsync();

    /// <summary>
    /// Get a bookmaker by name.
    /// </summary>
    Task<Bookmaker?> GetByNameAsync(string nom);

    /// <summary>
    /// Get a bookmaker with its offers.
    /// </summary>
    Task<Bookmaker?> GetWithOffersAsync(int id);
}
