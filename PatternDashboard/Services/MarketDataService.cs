using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace AIUI2025.Services
{
  public class MarketDataService
  {
    private readonly HttpClient _http;
    private readonly string _marketDataUrl;

    public MarketDataService(HttpClient http, IOptions<ApiSettings> options)
    {
      _http = http;
      _marketDataUrl = options.Value.MarketDataUrl;
    }

    public async Task<List<Candle>> GetLatestActiveWindowAsync( )
    {
      var now = DateTime.UtcNow; ;
      var scanStart = now;
      var scanStep = TimeSpan.FromMinutes(15);
      var windowSize = TimeSpan.FromMinutes(30);
      var maxLookback = TimeSpan.FromDays(5); // autonomy limit

      for (var offset = TimeSpan.Zero; offset <= maxLookback; offset += scanStep)
      {
        var windowEnd = scanStart - offset;
        var windowStart = windowEnd - windowSize;

        var candles = await GetCandlesAsync(windowStart, windowEnd);

        bool hasRecentActivity = candles.Any(c => c.Time >= windowEnd.AddMinutes(-30));

        if (hasRecentActivity)
          return candles;
        scanStep*=2;
      }

      return new List<Candle>(); // No activity found in lookback window
    }


    private async Task<List<Candle>> FetchCandles(DateTime start, DateTime end)
    {
      string baseUrl = $"{_marketDataUrl}/candles/1m";
      string url = $"{baseUrl}?start={Uri.EscapeDataString(start.ToString("s"))}&end={Uri.EscapeDataString(end.ToString("s"))}";
      Debug.WriteLine($"{start} {end}");

      var candles = await _http.GetFromJsonAsync<List<Candle>>(url);
      return candles ?? new List<Candle>();
    }

    public async Task<List<Candle>> GetCandlesAsync(DateTime start, DateTime end)
    {
        return await FetchCandles(start, end);
    }
  }
}