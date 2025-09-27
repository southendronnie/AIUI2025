public class StrategyConfig
{
  public int StopLossPips { get; set; }
  public int TakeProfitPips { get; set; }
  public decimal PipSize { get; set; }
  public string Timeframe { get; set; } // e.g. "1m", "5m", "1h"
  public decimal SpreadCost { get; set; } // in pips
  public decimal CommissionPerTrade { get; set; } // fixed cost
  public decimal SlippagePips { get; set; } // optional realism
  public string Instrument { get; set; } = "GBP_USD";
  public string Granularity { get; set; } = "M15";
  public decimal TradeCost { get; set; } = 0.5m;
  public int MaxLookahead { get; set; } = 10;
}
