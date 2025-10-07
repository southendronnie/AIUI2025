public interface IBacktestService
{
  Task<List<Candle>> LoadCandles(DateTime start, DateTime end, StrategyConfig config);
  Task<BacktestResult> RunBacktestAsync(StrategyConfig config, DateTime start, DateTime end);
}

