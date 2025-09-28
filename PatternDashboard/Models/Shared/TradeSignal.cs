using System;

public class TradeSignal
{
  public DateTime Time { get; set; }                 // Timestamp of signal
  public string Symbol { get; set; } = "GBP/USD";    // Instrument
  public string Pattern { get; set; } = "";          // e.g. Engulfing, Pin Bar
  public string Direction { get; set; } = "";        // Buy or Sell
  public double Confidence { get; set; } = 1.0;      // Optional scoring
  public string Source { get; set; } = "SignalService"; // Origin module
}