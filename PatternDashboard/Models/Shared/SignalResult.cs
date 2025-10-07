  public class SignalResult
  {
  private PatternType value;
  private double v;

  public SignalResult(DateTime time, PatternType value, string direction, double Confidence)
  {
    this.Time = time;
    this.value = value;
    this.Direction = direction;
    this.Pattern = value;
    this.Confidence = Confidence;
  }

  public DateTime Time { get; set; }              // UTC timestamp of signal
    public string Instrument { get; set; }          // e.g. "EUR_USD"
    public PatternType Pattern { get; set; }                // e.g. "PinBar", "Engulfing"
    public double Confidence { get; set; }          // 0.0–1.0 score
    public int Index { get; set; }                  // Candle index (optional)
    public string Direction { get; set; }           // "buy" or "sell"
    public string Strategy { get; set; }            // Strategy name or mode
    public decimal SL { get; set; }                 // Stop loss in pips
    public decimal TP { get; set; }                 // Take profit in pips
    public bool IsDryRun { get; set; }              // True if simulated
  public decimal TrendSlope { get; internal set; }
  public decimal VolatilityRatio { get; internal set; }
  public decimal Score { get; internal set; }
  public decimal Sma { get; internal set; }
  public decimal WinLossRatio { get; internal set; }
}
