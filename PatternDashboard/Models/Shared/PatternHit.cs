public class PatternHit
{
  public DateTime Time { get; set; }
  public string Type { get; set; } = string.Empty;
  public double Confidence { get; set; }

  public PatternHit() { }

  public PatternHit(DateTime time, string type, double confidence)
  {
    Time = time;
    Type = type;
    Confidence = confidence;
  }
}