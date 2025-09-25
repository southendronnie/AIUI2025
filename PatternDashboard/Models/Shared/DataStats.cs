
public class DataStats
{
  public int TotalCandles { get; set; }
  public int ExpectedCandles { get; set; }
  public int GapCount { get; set; }
  public double CoveragePercent => ExpectedCandles == 0 ? 0 : (double)TotalCandles / ExpectedCandles * 100;


  public DateTime StartTime { get; set; }
  public DateTime EndTime { get; set; }
}
