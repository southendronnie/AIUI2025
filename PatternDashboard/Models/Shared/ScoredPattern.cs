
public class ScoredPattern
{
  public DateTime Time { get; set; }                 // Timestamp of pattern
  public string Type { get; set; } = "";             // e.g. Bullish Engulfing
  public string Direction { get; set; } = "";        // Buy or Sell
  public double Confidence { get; set; }             // Raw confidence score
  public double Score { get; set; }                  // Final weighted score
  public StrategyConfig Config { get; set; } = new(); // Strategy context
  public List<Candle> FutureCandles { get; set; } = new(); // Injected for simulation
}