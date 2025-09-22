
public class ScoredPattern
{
  public DateTime Time { get; set; }
  public string Type { get; set; } = string.Empty;
  public double Confidence { get; set; }
  public double Score { get; set; }
}