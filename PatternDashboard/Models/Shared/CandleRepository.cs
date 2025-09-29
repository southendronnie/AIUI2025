using System.Text.Json;

public class CandleRepository : ICandleRepository
{
  private readonly string _dataPath;

  public CandleRepository(IConfiguration config)
  {
    _dataPath = Path.Combine(AppContext.BaseDirectory, "CandleData");
  }

  public async Task<List<Candle>> GetAllCandlesAsync()
  {
    if (!File.Exists(_dataPath))
    {
      Console.WriteLine($" Candle file not found: {_dataPath}");
      return new List<Candle>();
    }

    var json = await File.ReadAllTextAsync(_dataPath);
    return JsonSerializer.Deserialize<List<Candle>>(json)
           ?.OrderBy(c => c.Time)
           .ToList() ?? new List<Candle>();
  }

  public async Task<List<Candle>> GetCandlesAsync(DateTime start, DateTime end)
  {
    var all = await GetAllCandlesAsync();
    return all.Where(c => c.Time >= start && c.Time <= end).ToList();
  }
  public async Task<List<Candle>> LoadAsync()
  {
    return await GetAllCandlesAsync();
  }
}