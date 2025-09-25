
public class BacktestResult
{
  public List<SimulatedTrade> Trades { get; set; } = new();
  public int TotalTrades => Trades.Count;
  public int Wins => Trades.Count(t => t.Outcome == "Win");
  public int Losses => Trades.Count(t => t.Outcome == "Loss");
  public double WinRate => TotalTrades == 0 ? 0 : Wins / (double)TotalTrades;
  public double TotalProfit => Trades.Sum(t => t.Profit);
  public double MaxDrawdown { get; set; } = 0; // Optional: calculate from equity curve
  public List<EquityPoint> EquityCurve { get; set; } = new(); // Optional: for charting
}