public class SimulatedTrade
{
  public DateTime Time { get; set; }
  public string Direction { get; set; } = "";
  public double Entry { get; set; }
  public double Exit { get; set; }
  public string Outcome { get; set; } = ""; // Win, Loss, Breakeven
  public TimeSpan Duration { get; set; }
  public double Profit => Direction == "Buy" ? Exit - Entry : Entry - Exit;
  public string PatternType { get; set; } = ""; // e.g. "Bullish Engulfing"

}
