namespace BetTracker.Core.Models;

public class Offre
{
    public int Id { get; set; }
    public int BookmakerId { get; set; }
    public TypeOffre Type { get; set; }
    public decimal MontantBonus { get; set; }
    public decimal CoteMinimum { get; set; }
    public StatutOffre Statut { get; set; } = StatutOffre.AFaire;
    public DateTime DateCreation { get; set; } = DateTime.Now;
    public DateTime? DateTerminaison { get; set; }

    public Bookmaker Bookmaker { get; set; } = null!;
    public ICollection<Calcul> Calculs { get; } = new List<Calcul>();
    public ICollection<MouvementBankroll> Mouvements { get; } = new List<MouvementBankroll>();
}
