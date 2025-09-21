using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PatternLab.Extractors
{
  public static class PatternExtractor
  {
    public static IEnumerable<PricePattern> Extract(decimal[] prices, int windowSize = 5)
    {
      for (int i = 0; i <= prices.Length - windowSize; i++)
      {
        var slice = prices.Skip(i).Take(windowSize).ToArray();
        var direction = "";

        for (int j = 1; j < windowSize; j++)
        {
          var delta = slice[j] - slice[j - 1];
          direction += delta > 0 ? "↑" : delta < 0 ? "↓" : "→";
        }

        yield return new PricePattern { StartIndex = i, Direction = direction };
      }
    }
  }
}
