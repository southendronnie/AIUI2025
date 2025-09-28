using System.Text.Json;

public class OandaCandleService
{
  private readonly ILogger<OandaCandleService> _logger;

  public OandaCandleService(ILogger<OandaCandleService> logger)
  {
    _logger = logger;
  }

  public void ParseCandleJson(string json, string timeframe = "M1")
  {
    try
    {
      using var doc = JsonDocument.Parse(json);
      if (!doc.RootElement.TryGetProperty("candles", out var candles))
      {
        _logger.LogWarning("Missing 'candles' property in JSON.");
        return;
      }

      foreach (var candle in candles.EnumerateArray())
      {
        if (!candle.GetProperty("complete").GetBoolean()) continue;

        var time = candle.GetProperty("time").GetDateTime();
        var mid = candle.GetProperty("mid");

        var open = mid.GetProperty("o").GetDecimal();
        var high = mid.GetProperty("h").GetDecimal();
        var low = mid.GetProperty("l").GetDecimal();
        var close = mid.GetProperty("c").GetDecimal();

        var c = new Candle
        {
          Time = time,
          Open = (double)open,
          High = (double)high,
          Low = (double)low,
          Close = (double)close
        };

        AppendCandle(timeframe, c);
      }
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Failed to parse OANDA candle JSON.");
    }
  }

  private void AppendCandle(string timeframe, Candle candle)
  {
    // TODO: Implement your logic to store or forward the candle
    _logger.LogInformation($"Appended {timeframe} candle: {candle.Time} O:{candle.Open} H:{candle.High} L:{candle.Low} C:{candle.Close}");
  }
}