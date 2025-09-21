using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AIUI2025.Services
{
  public class MarketDataService
  {
    private readonly HttpClient _http;

    public MarketDataService(HttpClient http)
    {
      _http = http;
    }

    public async Task<List<Candle>> GetLatestActiveWindowAsync()
    {
      var now = DateTime.UtcNow;
      var scanStart = now;
      var scanStep = TimeSpan.FromMinutes(15);
      var windowSize = TimeSpan.FromMinutes(30);
      var maxLookback = TimeSpan.FromDays(2); // autonomy limit

      for (var offset = TimeSpan.Zero; offset <= maxLookback; offset += scanStep)
      {
        var end = scanStart - offset;
        var start = end - windowSize;

        var candles = await FetchCandles(start, end);

        bool hasRecentActivity = candles.Any(c => c.Time >= end.AddMinutes(-13));

        if (hasRecentActivity)
          return candles;
      }

      return new List<Candle>(); // No activity found in lookback window
    }


    private async Task<List<Candle>> FetchCandles(DateTime start, DateTime end)
    {
      string baseUrl = "https://fxai2-hrgzeve9dka0aqg3.canadacentral-01.azurewebsites.net/api/candles/1m";
      string url = $"{baseUrl}?start={Uri.EscapeDataString(start.ToString("s"))}&end={Uri.EscapeDataString(end.ToString("s"))}";

      var candles = await _http.GetFromJsonAsync<List<Candle>>(url);
      return candles ?? new List<Candle>();
    }
  }
}