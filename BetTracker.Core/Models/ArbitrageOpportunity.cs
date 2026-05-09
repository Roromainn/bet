namespace BetTracker.Core.Models;

/// <summary>
/// Represents an arbitrage opportunity between two bookmakers.
/// </summary>
public class ArbitrageOpportunity
{
    public int Id { get; set; }
    public int Bookmaker1Id { get; set; }
    public int Bookmaker2Id { get; set; }
    public decimal ProfitMargin { get; set; }
    public DateTime CreatedAt { get; set; }
}
