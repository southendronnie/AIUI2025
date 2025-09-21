

public class PatternService
  {
    public List<PatternHit> ExtractPatterns(List<Candle> candles)
    {
      var hits = new List<PatternHit>();

      for (int i = 1; i < candles.Count; i++)
      {
        var prev = candles[i - 1];
        var current = candles[i];

        if (prev.Close < prev.Open && current.Close > current.Open &&
            current.Open < prev.Close && current.Close > prev.Open)
        {
          hits.Add(new PatternHit
          {
            Time = current.Time,
            Type = "Buy E",
            Confidence = 0.85
          });
        }

        if (prev.Close > prev.Open && current.Close < current.Open &&
            current.Open > prev.Close && current.Close < prev.Open)
        {
          hits.Add(new PatternHit
          {
            Time = current.Time,
            Type = "Sell E",
            Confidence = 0.85
          });
        }
      }

      return hits;
    }

    public string GetSummary(List<PatternHit> hits)
    {
      if (hits == null || hits.Count == 0)
        return "No patterns detected.";

      var grouped = hits
          .GroupBy(h => h.Type)
          .Select(g => $"{g.Key}: {g.Count()} hits")
          .ToList();

      return $"Detected Patterns:\n" + string.Join("\n", grouped);
    }
  public string GetSummary(List<PatternHit> hits, TimeSpan windowSize)
  {
    if (hits == null || hits.Count == 0)
      return "No patterns detected.";

    var cutoff = DateTime.UtcNow - windowSize;
    var recentHits = hits.Where(h => h.Time >= cutoff).ToList();

    if (recentHits.Count == 0)
      return $"No patterns detected in the last {windowSize.TotalMinutes} minutes.";

    var grouped = recentHits
        .GroupBy(h => h.Type)
        .Select(g => $"{g.Key}: {g.Count()} hits")
        .ToList();

    return $"Recent Patterns:\n" + string.Join("\n", grouped);
  }
}
