using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Net.Http.Headers;
using System.Text.Json;

public class OandaCandleService : IOandaCandleService
{
  private readonly ILogger<OandaCandleService> _logger;
  private readonly CandleStore _store;
  private bool isPractice = true; // Set to false for live account
  private string token;
  private string accountId;



  public OandaCandleService(CandleStore store, string accountId, string token, bool isPractice)
  {
    _store = store;
    this.isPractice = isPractice;
    this.token = token ?? throw new ArgumentNullException(nameof(token));
    this.accountId = accountId ?? throw new ArgumentNullException(nameof(accountId));
  }


  public void ImportCandlesFromJson(string json, DateTime date)
  {
    try
    {
      using var doc = JsonDocument.Parse(json);
      if (!doc.RootElement.TryGetProperty("candles", out var candles))
      {
        //_logger.LogWarning("Missing 'candles' property in JSON.");
        return;
      }
      string granularity = doc.RootElement.GetProperty("granularity").ToString();
      string instrument = doc.RootElement.GetProperty("instrument").ToString();
      var list = new List<Candle>();

      foreach (var candle in candles.EnumerateArray())
      {
        if (!candle.GetProperty("complete").GetBoolean()) continue;

        var time = candle.GetProperty("time").GetDateTime();
        var mid = candle.GetProperty("mid");

        var open = decimal.Parse(mid.GetProperty("o").GetString());
        var high = decimal.Parse(mid.GetProperty("h").GetString());
        var low = decimal.Parse(mid.GetProperty("l").GetString());
        var close = decimal.Parse(mid.GetProperty("c").GetString());

        list.Add(new Candle
        {
          Time = time,
          Open = (double)open,
          High = (double)high,
          Low = (double)low,
          Close = (double)close
        });
      }
      _store.SaveCandles(instrument, granularity, list,date );
      Debug.WriteLine($"Imported {list.Count} candles to {granularity} store.");
    }
    catch (Exception ex)
    {
      Debug.WriteLine(ex);

      //_logger.LogError(ex, "Failed to import candles from JSON.");
    }
  }
///v3/instruments/EUR_USD/candles?from=2025-09-25T00:00:00Z&to=2025-09-26T00:00:00Z&granularity=M1
  public async Task LoadHistoricalCandles(string instrument, string granularity, DateTime from, DateTime to)
{
  var domain = isPractice ? "api-fxpractice.oanda.com" : "api-fxtrade.oanda.com";
  var url = $"https://{domain}/v3/instruments/{instrument}/candles" +
            $"?from={from:yyyy-MM-ddTHH:mm:ssZ}" +
            $"&to={to:yyyy-MM-ddTHH:mm:ssZ}" +
            $"&granularity={granularity}" +
            $"&price=M"; // M = midpoint; use "B" or "A" for bid/ask

  using var http = new HttpClient();
  http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

  try
  {
    var json = await http.GetStringAsync(url);
      ImportCandlesFromJson(json, from); // You’ll need to define this method to handle candle data
  }
  catch (Exception ex)
  {
      Debug.WriteLine(ex);
      Debug.WriteLine(url);
      // Optional: log or retry
    }
  }
}