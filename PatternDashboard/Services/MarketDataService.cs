using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class MarketDataService
{
  private readonly HttpClient _http;

  public MarketDataService(HttpClient http)
  {
    _http = http;
  }

  public async Task<List<Candle>> GetCandlesAsync()
  {
    var url = "https://fxai2-hrgzeve9dka0aqg3.canadacentral-01.azurewebsites.net/api";
    var candles = await _http.GetFromJsonAsync<List<Candle>>(url);
    return candles ?? new List<Candle>();
  }
}