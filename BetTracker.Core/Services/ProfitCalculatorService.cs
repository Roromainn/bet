namespace BetTracker.Core.Services;

public class ProfitCalculatorService : IProfitCalculatorService
{
    public decimal CalculateNetProfit(decimal backStake, decimal backOdds, decimal layStake, decimal layOdds)
    {
        var backProfit = backStake * (backOdds - 1);
        var layLiability = CalculateLiability(layStake, layOdds);
        return backProfit - layLiability;
    }

    public decimal CalculateLiability(decimal layStake, decimal layOdds)
    {
        return layStake * (layOdds - 1);
    }
}
