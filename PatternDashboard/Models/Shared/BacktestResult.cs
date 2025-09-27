public class BacktestResult
{
    public List<TradeResult> Trades { get; set; } = new();
    public int TotalTrades => Trades.Count;
    public int Wins => Trades.Count(t => t.NetPnL > 0);
    public int Losses => Trades.Count(t => t.NetPnL < 0);
    public double WinRate => TotalTrades == 0 ? 0 : Wins / (double)TotalTrades;
    public double TotalProfit => Trades.Sum(t => (double)t.NetPnL);
    public double MaxDrawdown { get; set; } = 0;
    public List<EquityPoint> EquityCurve { get; set; } = new();
} 