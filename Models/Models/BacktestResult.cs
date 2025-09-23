using System.Collections.Generic;

namespace FXAI.Models;

public class BacktestResult
{
    public int TotalTrades { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public double NetProfit { get; set; }
    public double MaxDrawdown { get; set; }
    public List<SimulatedTrade> Trades { get; set; } = new();

    public double WinRate => TotalTrades == 0 ? 0 : (double)Wins / TotalTrades;
}
