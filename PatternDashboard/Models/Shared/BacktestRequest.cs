
public class BacktestRequest
{
  public DateTime Start { get; set; }
  public DateTime End { get; set; }
  public StrategyConfig Config { get; set; } = new();
}