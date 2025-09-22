public class TickService
{
  private readonly HttpClient _http;

  public TickService(HttpClient http)
  {
    _http = http;
  }

  public async Task<List<PriceTick>> GetTicksAsync(DateTime start, DateTime end)
  {
    var url = $"https://fxai2-hrgzeve9dka0aqg3.canadacentral-01.azurewebsites.net/api/ticks?start={start:s}&end={end:s}";
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