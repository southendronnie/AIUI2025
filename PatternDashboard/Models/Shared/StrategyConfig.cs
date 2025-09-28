public class StrategyConfig
{
  public string Instrument { get; set; } = "GBP_USD";
  public string Granularity { get; set; } = "M1";

  // Use int for pip counts
  public int StopLossPips { get; set; } = 20;
  public int TakeProfitPips { get; set; } = 40;
  public int MaxLookahead { get; set; } = 10;
  public int Mode { get; set; } = 1;

  // Use decimal for price-related values
  public decimal PipSize { get; set; } = 0.0001m;
  public decimal SpreadCost { get; set; } = 1.0m;
  public decimal CommissionPerTrade { get; set; } = 0.0m;
  public decimal SlippagePips { get; set; } = 1m;

}
public enum StrategyMode: int
{
  SlTp,
  PatternBased
}

