public class CandleBuilder
{
  private readonly TickService _tickService;

  public CandleBuilder(TickService tickService)
  {
    _tickService = tickService;
  }

  public async Task<List<Candle>> BuildCandlesAsync(DateTime start, DateTime end, TimeSpan interval)
  {
    var ticks = await _tickService.GetTicksAsync(start, end);
    var candles = new List<Candle>();

    var grouped = ticks
        .GroupBy(t => new DateTime((t.Time.Ticks / interval.Ticks) * interval.Ticks))
        .OrderBy(g => g.Key);

    foreach (var group in grouped)
    {
      var ordered = group.OrderBy(t => t.Time).ToList();
      var candle = new Candle
      {
        Time = group.Key,
        Open = (double)ordered.First().Mid,
        High = (double)ordered.Max(t => t.Mid),
        Low = (double)ordered.Min(t => t.Mid),
        Close = (double)ordered.Last().Mid,
        Volume = ordered.Count
      };
      candles.Add(candle);
    }

    return candles;
  }
}