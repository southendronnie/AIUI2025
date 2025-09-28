public class StrategyConfig
{
    public string Instrument { get; set; }    = "GBP_USD";
  public string Granularity { get; set; } = "M1";
  public int StopLossPips { get; set; } = 20;
  public int TakeProfitPips { get; set; } =40;
  public decimal PipSize { get; set; } = 0.0001M;
  public decimal SpreadCost { get; set; } // in pips
  public decimal CommissionPerTrade { get; set; } // fixed cost
  public decimal SlippagePips { get; set; } // optional realism
  public int MaxLookahead { get; set; } = 10;
  public StrategyMode Mode { get; set; }
}

public enum StrategyMode
{
    SlTp,
    PatternBased
}

