using PatternDashboard.Models;
using PatternDashboard.Stats;
using PatternLab.Extractors;

namespace PatternLab.Services
{
  public class PatternService
  {
    public IEnumerable<PricePattern> GetPatterns(decimal[] prices, int windowSize = 4)
    {
      return PatternExtractor.Extract(prices, windowSize).ToList();
    }

    public IEnumerable<(string Direction, int Count)> GetSummary(decimal[] prices, int windowSize = 4)
    {
      var stats = new PatternStats();
      foreach (var pattern in PatternExtractor.Extract(prices, windowSize))
        stats.Add(pattern);

      return stats.Summarize();
    }
  }
}
