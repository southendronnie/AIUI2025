using FXAI.Core.Diagnostics;
using FXAI.Core.Patterns;
using OandaDataApi.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

public static class SignalService
{
  public static SignalResult? ShouldEnter(List<Candle> candles, int index, StrategyConfig config)
  {
    if (index < 10) return null;

    var prev = candles[index - 1];
    var curr = candles[index];

    var pattern = PatternService.DetectPattern(prev, curr);
    if (pattern == null) return null;

    var sma = candles.Skip(index - 10).Take(10).Average(c => c.Close);
    if ((pattern == PatternType.BullishEngulfing || pattern == PatternType.BullishPinBar) && curr.Close < sma)
      return null;

    if ((pattern == PatternType.BearishEngulfing || pattern == PatternType.BearishPinBar) && curr.Close > sma)
      return null;

    var range = curr.High - curr.Low;
    var avgRange = candles.Skip(index - 10).Take(10).Average(c => c.High - c.Low);
    if (range < avgRange * 0.8m)
      return null;

    if (!StrategyPatternMap.ModeMap.TryGetValue(config.Mode, out var validPatterns))
      return null;

    foreach (var (patternType, direction) in validPatterns)
    {
      if (pattern == patternType)
      {
        double confidence = patternType switch
        {
          PatternType.BullishEngulfing or PatternType.BearishEngulfing => PatternLibrary.CalculateEngulfingConfidence(curr, prev),
          PatternType.BullishPinBar or PatternType.BearishPinBar => PatternLibrary.CalculatePinBarConfidence(curr),
          _ => 0.0
        };

        return new SignalResult(curr.Time, pattern.Value, direction, confidence);
      }
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

