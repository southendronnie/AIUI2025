
public class TradeSimulator : ITradeSimulator
{
  public SimulatedTrade? Simulate(Candle entryCandle, ScoredPattern pattern)
  {
    bool hitTP = false;
    bool hitSL = false;

    if (entryCandle == null || pattern == null)
      return null;

      decimal entryPrice = (decimal)entryCandle.Open;
      decimal stopLoss = pattern.Direction == "Buy"
    ? entryPrice - (decimal)(pattern.Config.StopLossPips * 0.0001)
    : entryPrice + (decimal)(pattern.Config.StopLossPips * 0.0001);


    double takeProfit = (double)(pattern.Direction == "Buy"
        ? entryPrice + (decimal)(pattern.Config.TakeProfitPips * 0.0001)
        : entryPrice - (decimal)(pattern.Config.TakeProfitPips * 0.0001));

    foreach (var candle in pattern.FutureCandles)
    {

      if (hitTP || hitSL)
      {
        return new SimulatedTrade
        {
          Time = entryCandle.Time,
          Entry = (double)entryPrice,
          Exit = hitTP ? takeProfit : (double)stopLoss,
          Direction = pattern.Direction,
          Outcome = hitTP ? "Win" : "Loss",
          Duration = candle.Time - entryCandle.Time
        };
      }
    }

    return null;
  }
}