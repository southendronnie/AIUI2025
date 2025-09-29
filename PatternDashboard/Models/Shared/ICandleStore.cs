using System.Text.Json;

public interface ICandleStore
{
   void SaveCandles(string instrument, string granularity, List<Candle> candles, DateTime date);
   Task<List<Candle>> GetCandles(string instrument, string granularity, DateOnly date);
}