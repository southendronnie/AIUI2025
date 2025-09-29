using System.Threading.Tasks;

public class BacktestService
{
    private readonly ICandleRepository _candleRepo;
    private readonly IPatternService _patternService;
    private readonly ITradeSimulator _simulator;
    private readonly CandleStore _store;

    public BacktestService(
        ICandleRepository candleRepo,
        IPatternService patternService,
        ITradeSimulator simulator,
        CandleStore store)
    {
        _candleRepo = candleRepo ?? throw new ArgumentNullException(nameof(candleRepo));
        _patternService = patternService ?? throw new ArgumentNullException(nameof(patternService));
        _simulator = simulator ?? throw new ArgumentNullException(nameof(simulator));
        _store = store ?? throw new ArgumentNullException(nameof(store));
    }

    public async Task<BacktestResult> Run(DateTime start, DateTime end, StrategyConfig config)
    {
    var candles = await LoadCandles(
        start,
        end,
        config
    );

    return  BacktestEngine.Run(candles, start, end, config);
  }



  public async Task<List<Candle>> LoadCandles(DateTime start, DateTime end, StrategyConfig config)
  {
    var allCandles = new List<Candle>();
    var current = start;
    var step = TimeSpan.FromDays(1);

    while (current < end)
    {
      var next = current.Add(step);
      if (next > end) next = end;

      var candles = await _store.GetCandles(config.Instrument, config.Granularity, DateOnly.FromDateTime(current));
      if (candles != null && candles.Any())
        allCandles.AddRange(candles);

      current = next;
    }

    return allCandles;
  }

  private async Task<BacktestResult> Run(List<Candle> candles, DateTime start, DateTime end, StrategyConfig config)
  {

        var filtered = candles.Where(c => c.Time >= start && c.Time <= end).ToList();

        var scoredPatterns = new List<ScoredPattern>();
        var trades = new List<SimulatedTrade>();

        for (int i = 0; i < filtered.Count; i++)
        {
            var candle = filtered[i];
            var future = filtered.Skip(i + 1).Take(config.MaxLookahead).ToList();

            var hit = SignalService.ShouldEnter(future, i, config);
            if (hit != null)
            {
                // Convert SignalResult to ScoredPattern
                var scoredPattern = new ScoredPattern
                {
                    Time = hit.Time,
                    Type = hit.Pattern.ToString(),
                    Direction = hit.Direction,
                    Confidence = 0, // Set as needed
                    Score = 0,      // Set as needed
                    Config = config,
                    FutureCandles = future
                };
                var trade = _simulator.Simulate(candle, scoredPattern);
                if (trade != null) trades.Add(trade);
                scoredPatterns.Add(scoredPattern);
            }
        }

        var tradeResults = trades.Select(t => new TradeResult
        {
            EntryTime = t.Time,
            ExitTime = t.Time + t.Duration,
            EntryPrice = (decimal)t.Entry,
            ExitPrice = (decimal)t.Exit,
            RawPnL = (decimal)t.Profit,
            NetPnL = (decimal)t.Profit,
            Cost = 0
        }).ToList();

        return new BacktestResult
        {
            Trades = tradeResults,
            EquityCurve = EquityBuilder.Build(trades),
            MaxDrawdown = (double)DrawdownCalculator.Calculate(trades)
        };
    }
}
