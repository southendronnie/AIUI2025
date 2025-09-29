public interface ICandleRepository
{
  Task<List<Candle>> GetAllCandlesAsync();
  Task<List<Candle>> GetCandlesAsync(DateTime start, DateTime end);
  Task<List<Candle>> LoadAsync(); // optional alias
}