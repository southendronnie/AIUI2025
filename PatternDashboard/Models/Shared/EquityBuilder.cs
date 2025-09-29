
public static class EquityBuilder
{
  public static List<EquityPoint> Build(List<SimulatedTrade> trades)
  {
    var curve = new List<EquityPoint>();
    decimal balance = 0;

    foreach (var trade in trades.OrderBy(t => t.Time))
    {
      balance += (decimal)trade.Profit;
      curve.Add(new EquityPoint
      {
        Time = trade.Time,
        Balance = (double)balance
      });
    }

    return curve;
  }
}