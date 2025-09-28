using System;
using System.Collections.Generic;
using System.Threading.Tasks;

  public static class PatternService
  {
    public static PatternType? DetectPattern(Candle prev, Candle curr)
    {
      // Basic Engulfing logic
      if (prev.Close < prev.Open && curr.Close > curr.Open && curr.Close > prev.Open && curr.Open < prev.Close)
        return PatternType.BullishEngulfing;

      if (prev.Close > prev.Open && curr.Close < curr.Open && curr.Close < prev.Open && curr.Open > prev.Close)
        return PatternType.BearishEngulfing;

      // Basic Pin Bar logic
      var bodySize = Math.Abs(curr.Close - curr.Open);
      var candleRange = curr.High - curr.Low;
      var upperWick = curr.High - Math.Max(curr.Close, curr.Open);
      var lowerWick = Math.Min(curr.Close, curr.Open) - curr.Low;

      if (bodySize < candleRange * 0.3)
      {
        if (lowerWick > upperWick * 2)
          return PatternType.BullishPinBar;

        if (upperWick > lowerWick * 2)
          return PatternType.BearishPinBar;
      }

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
