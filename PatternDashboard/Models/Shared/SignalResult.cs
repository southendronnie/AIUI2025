public class SignalResult
{
  public DateTime Time { get; set; }
  public PatternType Pattern { get; set; }
  public string Direction { get; set; }
  public double Confidence { get; set; }

  // UI-friendly aliases
  public string Type => Pattern.ToString();

  public SignalResult(DateTime time, PatternType pattern, string direction, double confidence = 1.0)
  {
    Time = time;
    Pattern = pattern;
    Direction = direction;
    Confidence = confidence;
  }
}
