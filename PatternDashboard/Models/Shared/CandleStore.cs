using System.Text.Json;
using System.Threading.Tasks;
using static CandleStore;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class CandleStore : ICandleStore
{
  private readonly string _rootPath;
  private  IOandaCandleService? _candleService;

  private readonly Dictionary<string, List<Candle>> _cache = new();

  public CandleStore(string rootPath, IOandaCandleService? candleService = null)
  {
    _rootPath = rootPath;
  }

  public async Task<List<Candle>> GetCandles(string instrument, string granularity, DateOnly date)
  {
    var key = $"candles-{instrument}-{granularity}-{date:D}.json";

    if (!_cache.TryGetValue(key, out var candles))
    {
      candles = LoadCandlesFromDisk(instrument, granularity, new DateTime(date, TimeOnly.MinValue));

      // If no candles loaded from disk, attempt to load from CandleService (if available)
      if (candles == null || candles.Count == 0)
      {
        _candleService = _candleService ?? Stat.Oanda;

        // Assuming ICandleService is available via DI or as a property
        if (_candleService != null)
        {
          await _candleService.LoadHistoricalCandles( instrument,  granularity, new DateTime(date, TimeOnly.MinValue), new  DateTime(date,TimeOnly.MinValue).AddDays(1));
          candles = LoadCandlesFromDisk(instrument, granularity, new DateTime(date, TimeOnly.MinValue));
        }
        else
        {
          candles = new List<Candle>();
        }
      }

      _cache[key] = candles;
    }

    return candles
        .Where(c => c.Time >= new DateTime(date, TimeOnly.MinValue).AddDays(0) && c.Time <= new DateTime(date, TimeOnly.MinValue).AddDays(1))
        .OrderBy(c => c.Time)
        .ToList();
  }

  private List<Candle> LoadCandlesFromDisk(string instrument, string granularity, DateTime date)
  {
    var path = Path.Combine(_rootPath, $"candles-{instrument}-{granularity}-{date:yyyy-MM-dd}.json");
    if (!File.Exists(path)) return new();

    var json = File.ReadAllText(path);
    return JsonSerializer.Deserialize<List<Candle>>(json, new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true
    }) ?? new();
  }

  public void SaveCandles(string instrument, string granularity, List<Candle> candles, DateTime date)
  {
    var path = Path.Combine(_rootPath, $"candles-{instrument}-{granularity}-{date:yyyy-MM-dd}.json");
    var json = JsonSerializer.Serialize(candles, new JsonSerializerOptions
    {
      WriteIndented = false
    });

    File.WriteAllText(path, json);
    _cache[$"candles-{instrument}-{granularity}-{date:D}.json"] = candles;
  }

  // Add a property for  CandleService (optional, depending on your architecture)

}