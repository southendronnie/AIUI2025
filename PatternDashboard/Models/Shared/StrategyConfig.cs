public class StrategyConfig
{
  internal readonly int MaxTradesPerRun = 2;

  public string Instrument { get; set; } = "GBP_USD";
  public string Granularity { get; set; } = "M15";

  // Use int for pip counts
  public int StopLossPips { get; set; } = 30;
  public int TakeProfitPips { get; set; } = 50;
  public int MaxLookahead { get; set; } = 5;
  public int Mode { get; set; } = 1;

  // Use decimal for price-related values
  public decimal PipSize { get; set; } = 0.0001m;
  public decimal SpreadCost { get; set; } = 0.0m;
  public decimal CommissionPerTrade { get; set; } = 0.0m;
  public decimal SlippagePips { get; set; } = 0m;
  public double MinConfidence { get; set; } = 0.7;
  public decimal MinTrendSlope { get; set; } = 0.0015m;

  public bool UseTrendFilter { get; set; } = true;
  public bool UseSmaFilter { get; set; } = true;
  public bool UseVolatilityFilter { get; set; } = true;
  public bool UseTimeFilter { get; set; } = false;

  public decimal MinWinLossRatio { get; set; } = 1.2m;
  public decimal MinSignalScore { get; set; } = 0.05m;




}

