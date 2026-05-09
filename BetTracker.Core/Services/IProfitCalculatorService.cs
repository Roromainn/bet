namespace BetTracker.Core.Services;

public interface IProfitCalculatorService
{
    decimal CalculateNetProfit(decimal backStake, decimal backOdds, decimal layStake, decimal layOdds);
    decimal CalculateLiability(decimal layStake, decimal layOdds);
}
