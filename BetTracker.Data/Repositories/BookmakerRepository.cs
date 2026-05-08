using Microsoft.EntityFrameworkCore;
using BetTracker.Core.Models;

namespace BetTracker.Data.Repositories;

/// <summary>
/// Repository for Bookmaker entity with specialized queries.
/// </summary>
public class BookmakerRepository : Repository<Bookmaker>, IBookmakerRepository
{
    private readonly BetTrackerContext _context;

    public BookmakerRepository(BetTrackerContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all active bookmakers.
    /// </summary>
    public async Task<IEnumerable<Bookmaker>> GetActiveAsync()
    {
        return await _context.Bookmakers
            .Where(b => b.Actif)
            .ToListAsync();
    }

    /// <summary>
    /// Get a bookmaker by name.
    /// </summary>
    public async Task<Bookmaker?> GetByNameAsync(string nom)
    {
        return await _context.Bookmakers
            .FirstOrDefaultAsync(b => b.Nom == nom);
    }

    /// <summary>
    /// Get a bookmaker with its offers.
    /// </summary>
    public async Task<Bookmaker?> GetWithOffersAsync(int id)
    {
        return await _context.Bookmakers
            .Include(b => b.Offres)
            .FirstOrDefaultAsync(b => b.Id == id);
    }
}
