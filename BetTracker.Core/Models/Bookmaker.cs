namespace BetTracker.Core.Models;

public class Bookmaker
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string UrlLogo { get; set; } = string.Empty;
    public bool Actif { get; set; }
    public DateTime DateActivation { get; set; }

    public ICollection<Offre> Offres { get; } = new List<Offre>();
    public ICollection<MouvementBankroll> Mouvements { get; } = new List<MouvementBankroll>();
}
