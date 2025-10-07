using OandaDataApi.Models.Shared;
using System;

public static class PatternService
{
  public static PatternType? DetectPattern(Candle prev, Candle curr)
  {
    if (IsBullishEngulfing(prev, curr)) return PatternType.BullishEngulfing;
    if (IsBearishEngulfing(prev, curr)) return PatternType.BearishEngulfing;
    if (IsBullishPinBar(curr)) return PatternType.BullishPinBar;
    if (IsBearishPinBar(curr)) return PatternType.BearishPinBar;
    if (IsMorningStar(prev, curr)) return PatternType.MorningStar;
    if (IsEveningStar(prev, curr)) return PatternType.EveningStar;
    if (IsInsideBar(prev, curr)) return PatternType.InsideBar;
    if (IsOutsideBar(prev, curr)) return PatternType.OutsideBar;

    return null;
  }

  private static bool IsBullishEngulfing(Candle prev, Candle curr)
  {
    return prev.Close < prev.Open &&
           curr.Close > curr.Open &&
           curr.Open < prev.Close &&
           curr.Close > prev.Open;
  }

  private static bool IsBearishEngulfing(Candle prev, Candle curr)
  {
    return prev.Close > prev.Open &&
           curr.Close < curr.Open &&
           curr.Open > prev.Close &&
           curr.Close < prev.Open;
  }

  private static bool IsBullishPinBar(Candle curr)
  {
    var body = Math.Abs(curr.Close - curr.Open);
    var tail = curr.Low - Math.Min(curr.Close, curr.Open);
    return tail > body * 2 && curr.Close > curr.Open;
  }

  private static bool IsBearishPinBar(Candle curr)
  {
    var body = Math.Abs(curr.Close - curr.Open);
    var wick = curr.High - Math.Max(curr.Close, curr.Open);
    return wick > body * 2 && curr.Close < curr.Open;
  }

  private static bool IsMorningStar(Candle prev, Candle curr)
  {
    return prev.Close < prev.Open &&
           curr.Open > prev.Close &&
           curr.Close > curr.Open &&
           (curr.Close - curr.Open) > (prev.Open - prev.Close) * 0.5m;
  }

  private static bool IsEveningStar(Candle prev, Candle curr)
  {
    return prev.Close > prev.Open &&
           curr.Open < prev.Close &&
           curr.Close < curr.Open &&
           (curr.Open - curr.Close) > (prev.Close - prev.Open) * 0.5m;
  }

  private static bool IsInsideBar(Candle prev, Candle curr)
  {
    return curr.High < prev.High && curr.Low > prev.Low;
  }

  private static bool IsOutsideBar(Candle prev, Candle curr)
  {
    return curr.High > prev.High && curr.Low < prev.Low;
  }
}