    public interface IOandaCandleService
    {
    public Task LoadHistoricalCandles(string instrument, string granularity, DateTime from, DateTime to);

    }
