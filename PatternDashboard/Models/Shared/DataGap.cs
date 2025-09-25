using System;

public class DataGap
{
  public DateTime Start { get; set; }
  public DateTime End { get; set; }
  public TimeSpan Duration => End - Start;
}

