public interface ITradeSimulator
{
  SimulatedTrade? Simulate(Candle entryCandle, ScoredPattern pattern);
}
