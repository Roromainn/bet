namespace BetTracker.Core.Models;

public class MouvementBankroll
{
    public int Id { get; set; }
    public int BookmakerId { get; set; }
    public int? OffreId { get; set; }
    public TypeMouvement Type { get; set; }
    public decimal Montant { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;

    public Bookmaker Bookmaker { get; set; } = null!;
    public Offre? Offre { get; set; }
}
