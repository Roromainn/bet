namespace BetTracker.Core.Models;

public class Execution
{
    public int Id { get; set; }
    public int CalculId { get; set; }
    public decimal MiseReelleBook1 { get; set; }
    public decimal MiseReelleBook2 { get; set; }
    public decimal CoteReelleBook1 { get; set; }
    public decimal CoteReelleBook2 { get; set; }
    public ResultatMatch ResultatMatch { get; set; }
    public decimal GainBrut { get; set; }
    public decimal EVReelle { get; set; }
    public DateTime DateExecution { get; set; } = DateTime.Now;

    public Calcul Calcul { get; set; } = null!;
}
