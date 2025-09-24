using Microsoft.Extensions.Options;

public class TickService
{
  private readonly HttpClient _http;

  private readonly string _marketDataUrl;

  public TickService(HttpClient http, IOptions<ApiSettings> options)
  {
    _http = http;
    _marketDataUrl = options.Value.MarketDataUrl;
  }

  public async Task<List<PriceTick>> GetTicksAsync(DateTime start, DateTime end)
  {
    var url = $"{_marketDataUrl}/ticks?start={start:s}&end={end:s}";
    return await _http.GetFromJsonAsync<List<PriceTick>>(url) ?? new();
  }
}

public class PriceTick
{
  public DateTime Time { get; set; }
  public decimal Bid { get; set; }
  public decimal Ask { get; set; }

  public decimal Mid => (Bid + Ask) / 2;
}