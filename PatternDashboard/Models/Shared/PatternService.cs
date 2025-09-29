
public class PatternService : IPatternService
{
  public static bool IsBullishEngulfing(Candle prev, Candle curr)
  {
    return prev.Close < prev.Open &&
           curr.Close > curr.Open &&
           curr.Close > prev.Open &&
           curr.Open < prev.Close;
  }

  public static bool IsBearishEngulfing(Candle prev, Candle curr)
  {
    return prev.Close > prev.Open &&
           curr.Close < curr.Open &&
           curr.Close < prev.Open &&
           curr.Open > prev.Close;
  }

  public static bool IsPinBar(Candle candle, decimal threshold = 0.66m)
  {
    var body = Math.Abs((decimal)(candle.Close - candle.Open));
    var range = (decimal)(candle.High - candle.Low);
    var tail = Math.Min((decimal)(candle.Open - candle.Low), (decimal)(candle.Close - candle.Low));
    var wick = Math.Min((decimal)(candle.High - candle.Open), (decimal)(candle.High - candle.Close));

    if (range == 0) return false;

    return (tail / range > threshold || wick / range > threshold) && body / range < (1 - threshold);
  }

  public static bool IsInsideBar(Candle prev, Candle curr)
  {
    return curr.High < prev.High && curr.Low > prev.Low;
  }

  public static PatternType? DetectPattern(Candle prev, Candle curr)
  {
    if (IsBullishEngulfing(prev, curr)) return PatternType.BullishEngulfing;
    if (IsBearishEngulfing(prev, curr)) return PatternType.BearishEngulfing;
    if (IsInsideBar(prev, curr)) return PatternType.BullishPinBar;
    if (IsPinBar(curr)) return PatternType.BearishPinBar;
      return null;
    }

    public static string GetSummary(List<Candle> candles)
    {
      int bullish = 0, bearish = 0, pins = 0;

      for (int i = 1; i < candles.Count; i++)
      {
        var pattern = DetectPattern(candles[i - 1], candles[i]);
        if (pattern == PatternType.BullishEngulfing || pattern == PatternType.BearishEngulfing)
          bullish += pattern == PatternType.BullishEngulfing ? 1 : 0;
        bearish += pattern == PatternType.BearishEngulfing ? 1 : 0;

        if (pattern == PatternType.BullishPinBar || pattern == PatternType.BearishPinBar)
          pins++;
      }

      return $"Bullish Engulfing: {bullish}, Bearish Engulfing: {bearish}, Pin Bars: {pins}";
    }

    public static Task<List<ScoredPattern>> GetScoredPatternsAsync(List<Candle> candles)
    {
      var scored = new List<ScoredPattern>();

      for (int i = 1; i < candles.Count; i++)
      {
        var pattern = DetectPattern(candles[i - 1], candles[i]);
        if (pattern != null)
        {
          scored.Add(new ScoredPattern
          {
            Time = candles[i].Time,
            //Pattern = pattern.Value,
            Confidence = 1.0 // Stub: always 100% for now
          });
        }
      }

      return Task.FromResult(scored);
    }
  }
