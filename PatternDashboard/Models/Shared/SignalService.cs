using System;
using System.Collections.Generic;

  public static class SignalService
  {
    public static SignalResult? ShouldEnter(List<Candle> candles, int index, StrategyConfig config)
    {
      if (index < 1) return null;

      var prev = candles[index - 1];
      var curr = candles[index];

      var pattern = PatternService.DetectPattern(prev, curr);
      if (pattern == null) return null;

      // Mode-based logic
      switch (config.Mode)
      {
        case 1: // Bullish Engulfing
          if (pattern == PatternType.BullishEngulfing)
            return new SignalResult(curr.Time, pattern.Value, "Long");
          break;

        case 2: // Bearish Engulfing
          if (pattern == PatternType.BearishEngulfing)
            return new SignalResult(curr.Time, pattern.Value, "Short");
          break;

        case 3: // Pin Bar (example)
          if (pattern == PatternType.BullishPinBar)
            return new SignalResult(curr.Time, pattern.Value, "Long");
          if (pattern == PatternType.BearishPinBar)
            return new SignalResult(curr.Time, pattern.Value, "Short");
          break;
      }

      return null;
    }

    public static List<SignalResult> ExtractSignals(List<Candle> candles, StrategyConfig config)
    {
      var signals = new List<SignalResult>();
      for (int i = 0; i < candles.Count; i++)
      {
        var signal = ShouldEnter(candles, i, config);
        if (signal != null)
          signals.Add(signal);
      }
      return signals;
    }
  }
