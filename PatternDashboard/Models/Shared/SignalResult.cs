public class SignalResult
{
  public DateTime Time { get; set; }
  public PatternType Pattern { get; set; }
  public string Direction { get; set; }

  // Optional extras for UI
  public string Type => Pattern.ToString();
  public double Confidence { get; set; } = 1.0; // Default to 100% confidence'

  public SignalResult(DateTime time, PatternType pattern, string direction)
  {
    Time = time;
    Pattern = pattern;
    Direction = direction;
  }
}
