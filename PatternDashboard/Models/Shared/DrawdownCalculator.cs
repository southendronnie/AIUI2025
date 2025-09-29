public static class DrawdownCalculator
{
  public static decimal Calculate(List<SimulatedTrade> trades)
  {
    var equity = EquityBuilder.Build(trades);
    decimal peak = 0;
    decimal maxDrawdown = 0;

    foreach (var point in equity)
    {
      if (point.Balance > (double)peak)
        peak = (decimal)point.Balance;

      var drawdown = peak - (decimal)point.Balance;
      if (drawdown > maxDrawdown)
        maxDrawdown = drawdown;
    }

    return maxDrawdown;
  }
}