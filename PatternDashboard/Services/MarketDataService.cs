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

    public async Task<List<Candle>> GetCandlesAsync(DateTime start, DateTime end)
    {
      string baseUrl = "https://fxai2-hrgzeve9dka0aqg3.canadacentral-01.azurewebsites.net/api/candles/1m";
      string url = $"{baseUrl}?start={Uri.EscapeDataString(start.ToString("s"))}&end={Uri.EscapeDataString(end.ToString("s"))}";

      var candles = await _http.GetFromJsonAsync<List<Candle>>(url);
      return candles ?? new List<Candle>();
    }
  }
}