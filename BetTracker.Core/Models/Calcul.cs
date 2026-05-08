namespace BetTracker.Core.Models;

public class Calcul
{
    public int Id { get; set; }
    public int OffreId { get; set; }
    public decimal CoteBook1 { get; set; }
    public decimal CoteBook2 { get; set; }
    public decimal MiseBook1 { get; set; }
    public decimal MiseBook2 { get; set; }
    public decimal PerteCouverture { get; set; }
    public decimal EVNette { get; set; }
    public bool GoNoGo { get; set; }
    public bool Valide { get; set; }
    public DateTime DateCalcul { get; set; } = DateTime.Now;

    public Offre Offre { get; set; } = null!;
    public Execution? Execution { get; set; }
}
