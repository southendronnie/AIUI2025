public class BacktestResult
{

  public Dictionary<string, int> SignalCounts { get; set; }
  public Dictionary<string, decimal> AvgPnLByPattern { get; set; }
  public string MostCommonSignal { get; set; }

  public List<TradeResult> Trades { get; set; } = new();
  public int TotalTrades => Trades.Count;
  public int Wins => Trades.Count(t => t.NetPnL > 0);
  public int Losses => Trades.Count(t => t.NetPnL < 0);
  public double WinRate => TotalTrades == 0 ? 0 : Wins / (double)TotalTrades;
  public double TotalProfit => (double)Trades.Sum(t => t.NetPnL);
  public double MaxDrawdown { get; set; } = 0;
  public List<EquityPoint> EquityCurve { get; set; } = new();
  public Dictionary<string, decimal> PnLByPattern { get; internal set; }
  public Dictionary<string, int> DirectionCounts { get; internal set; }
}