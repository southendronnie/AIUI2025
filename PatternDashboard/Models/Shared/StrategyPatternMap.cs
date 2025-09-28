namespace OandaDataApi.Models.Shared
{
  public class StrategyPatternMap
  {
    public static Dictionary<int, List<(PatternType Pattern, string Direction)>> ModeMap = new()
    {
      [1] = new List<(PatternType, string)>
        {
            (PatternType.BullishEngulfing, "Long")
        },
      [2] = new List<(PatternType, string)>
        {
            (PatternType.BearishEngulfing, "Short")
        },
      [3] = new List<(PatternType, string)>
        {
            (PatternType.BullishPinBar, "Long"),
            (PatternType.BearishPinBar, "Short")
        },
      [4] = new List<(PatternType, string)>
        {
            (PatternType.BullishEngulfing, "Long"),
            (PatternType.BearishEngulfing, "Short"),
            (PatternType.BullishPinBar, "Long"),
            (PatternType.BearishPinBar, "Short")
        }
    };
  }

}
