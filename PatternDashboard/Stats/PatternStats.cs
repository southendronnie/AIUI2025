using PatternDashboard.Models;

namespace PatternDashboard.Stats
{
  public class PatternStats
  {
    private readonly Dictionary<string, int> _counts = new();

    public void Add(PricePattern pattern)
    {
      if (!_counts.TryGetValue(pattern.Direction, out var count))
        _counts[pattern.Direction] = 1;
      else
        _counts[pattern.Direction] = count + 1;
    }

    public IEnumerable<(string Direction, int Count)> Summarize()
    {
      foreach (var kvp in _counts)
        yield return (kvp.Key, kvp.Value);
    }
  }
}