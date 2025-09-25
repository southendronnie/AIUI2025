
public class StrategyConfig
{
  public Dictionary<string, double> PatternWeights { get; set; } = new()
    {
        { "Bullish Engulfing", 1.00 },
        { "Bearish Engulfing", 0.95 },
        { "Pin Bar", 0.92 },
        { "Inside Bar", 0.90 }
    };

  public Dictionary<string, double> TimeFrameWeights { get; set; } = new()
    {
        { "5m", 1.00 },
        { "15m", 1.05 },
        { "1h", 1.10 }
    };

  public double StopLossPips { get; set; } = 20;
  public double TakeProfitPips { get; set; } = 40;

  public int MaxLookahead { get; set; } = 10; // 👈 This is the missing property
}