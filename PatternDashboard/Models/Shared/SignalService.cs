using OandaDataApi.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

public static class SignalService
{
  public static SignalResult? ShouldEnter(List<Candle> candles, int index, StrategyConfig config)
  {
    if (index < 10) return null; // Ensure enough candles for SMA

    var prev = candles[index - 1];
    var curr = candles[index];

    var pattern = PatternService.DetectPattern(prev, curr);
    if (pattern == null) return null;

    // Optional trend filter using 10-period SMA
    var sma = candles.Skip(index - 10).Take(10).Average(c => c.Close);
    if (pattern == PatternType.BullishEngulfing || pattern == PatternType.BullishPinBar)
      if (curr.Close < sma) return null;

    if (pattern == PatternType.BearishEngulfing || pattern == PatternType.BearishPinBar)
      if (curr.Close > sma) return null;

    var range = curr.High - curr.Low;
    var avgRange = candles.Skip(index - 10).Take(10).Average(c => c.High - c.Low);
    if (range < avgRange * 0.8) // Use double for both operands
      return null; // skip low-volatility setups

    if (!StrategyPatternMap.ModeMap.TryGetValue(config.Mode, out var validPatterns))
      return null;

    foreach (var (patternType, direction) in validPatterns)
    {
      if (pattern == patternType)
        return new SignalResult(curr.Time, pattern.Value, direction, 1.0);
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

