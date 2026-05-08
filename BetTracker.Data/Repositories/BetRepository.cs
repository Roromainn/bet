using Microsoft.EntityFrameworkCore;
using BetTracker.Core.Models;

namespace BetTracker.Data.Repositories;

/// <summary>
/// Repository for Offre (Bet) entity with specialized queries.
/// </summary>
public class BetRepository : Repository<Offre>, IBetRepository
{
    private readonly BetTrackerContext _context;

    public BetRepository(BetTrackerContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all active bets (offers).
    /// </summary>
    public async Task<IEnumerable<Offre>> GetActiveBetsAsync()
    {
        return await _context.Offres
            .Where(o => o.Statut == StatutOffre.EnCours)
            .Include(o => o.Bookmaker)
            .ToListAsync();
    }

    /// <summary>
    /// Get bets by bookmaker ID.
    /// </summary>
    public async Task<IEnumerable<Offre>> GetByBookmakerAsync(int bookmakerId)
    {
        return await _context.Offres
            .Where(o => o.BookmakerId == bookmakerId)
            .Include(o => o.Bookmaker)
            .ToListAsync();
    }

    /// <summary>
    /// Get a bet with its calculations.
    /// </summary>
    public async Task<Offre?> GetWithCalculsAsync(int id)
    {
        return await _context.Offres
            .Include(o => o.Calculs)
            .Include(o => o.Bookmaker)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    /// <summary>
    /// Get bets by type.
    /// </summary>
    public async Task<IEnumerable<Offre>> GetByTypeAsync(TypeOffre type)
    {
        return await _context.Offres
            .Where(o => o.Type == type)
            .Include(o => o.Bookmaker)
            .ToListAsync();
    }

    /// <summary>
    /// Get completed bets (offers).
    /// </summary>
    public async Task<IEnumerable<Offre>> GetCompletedBetsAsync()
    {
        return await _context.Offres
            .Where(o => o.Statut == StatutOffre.Terminee)
            .Include(o => o.Bookmaker)
            .ToListAsync();
    }
}
