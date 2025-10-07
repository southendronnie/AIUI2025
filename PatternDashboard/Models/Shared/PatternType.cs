public enum PatternType
{
  None = 0,
  BullishEngulfing,
  BearishEngulfing,
  BullishPinBar,
  BearishPinBar,
  MorningStar,
  EveningStar,
  InsideBar,
  OutsideBar
}


public static class PatternTypeExtensions
{
  public static bool IsBullish(this PatternType type) =>
      type == PatternType.BullishEngulfing || type == PatternType.BullishPinBar;

  public static bool IsBearish(this PatternType type) =>
      type == PatternType.BearishEngulfing || type == PatternType.BearishPinBar;

  public static bool IsPinBar(this PatternType type) =>
      type == PatternType.BullishPinBar || type == PatternType.BearishPinBar;

  public static bool IsEngulfing(this PatternType type) =>
      type == PatternType.BullishEngulfing || type == PatternType.BearishEngulfing;
}
