using System.Diagnostics;

public static class BacktestEngine
{
  public static BacktestResult Run(List<Candle> candles, DateTime start, DateTime end, StrategyConfig config)
  {
    var result = new BacktestResult();
    TradeResult? activeTrade = null;
    decimal balance = 0;
    decimal peakEquity = 0;

    for (int i = 0; i < candles.Count; i++)
    {
      var candle = candles[i];
      if (candle.Time < start || candle.Time > end)
        continue;

      if (activeTrade == null)
      {
        // 🔑 Use SignalService instead of inline IsEntrySignal
        var signal = SignalService.ShouldEnter(candles, i, config);
        if (signal != null)
        {
          var slippage = config.SlippagePips * config.PipSize;
          var spread = config.SpreadCost * config.PipSize;
          var totalCost = spread + config.CommissionPerTrade + slippage;

          activeTrade = new TradeResult
          {
            EntryTime = candle.Time,
            EntryPrice = (decimal)candle.Close,
            Signal = signal.Pattern.ToString(),
            Direction = signal.Direction,
            SpreadCost = spread,
            SlippageCost = slippage,
            Cost = totalCost
          };
        }
      }
      else
      {
        // Manage open trade
        var sl = activeTrade.EntryPrice - config.StopLossPips * config.PipSize;
        var tp = activeTrade.EntryPrice + config.TakeProfitPips * config.PipSize;

        int maxIndex = Math.Min(i + config.MaxLookahead, candles.Count - 1);
        for (int j = i; j <= maxIndex; j++)
        {
          var future = candles[j];
          if ((decimal)future.Low <= sl)
          {
            activeTrade.ExitTime = future.Time;
            activeTrade.ExitPrice = sl;
            activeTrade.RawPnL = -config.StopLossPips * config.PipSize;
            break;
          }
          else if ((decimal)future.High >= tp)
          {
            activeTrade.ExitTime = future.Time;
            activeTrade.ExitPrice = tp;
            activeTrade.RawPnL = config.TakeProfitPips * config.PipSize;
            break;
          }
        }

        if (activeTrade.ExitTime != default)
        {
          activeTrade.NetPnL = activeTrade.RawPnL - activeTrade.Cost;
          result.Trades.Add(activeTrade);
          balance += activeTrade.NetPnL;
          activeTrade = null;
        }
      }

      // Equity + drawdown tracking
      decimal equity = 0;
      decimal peak = 0;
      decimal maxDrawdown = 0;

      var signalCounts = new Dictionary<string, int>();
      var pnlByPattern = new Dictionary<string, List<decimal>>();

      foreach (var trade in result.Trades)
      {
        var key = trade.Signal;
        signalCounts[key] = signalCounts.GetValueOrDefault(key) + 1;
        pnlByPattern.TryAdd(key, new List<decimal>());
        pnlByPattern[key].Add(trade.NetPnL);
      }


      foreach (var trade in result.Trades)
      {
        equity += trade.NetPnL;
        peak = Math.Max(peak, equity);
        maxDrawdown = Math.Max(maxDrawdown, peak - equity);
      }

      result.MaxDrawdown = (double)maxDrawdown;

      result.EquityCurve.Add(new EquityPoint
      {
        Time = candle.Time,
        Balance = (double)balance,
        Equity = (double)equity
      });
    }

    return result;
  }
}