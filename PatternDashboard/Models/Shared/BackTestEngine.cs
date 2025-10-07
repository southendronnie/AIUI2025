using FXAI.Core.Control;
using FXAI.Core.Diagnostics;
using FXAI.Core.Execution;
using FXAI.Core.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Wilshire.Benchmarks.Wplus.Engine.Helper;

namespace FXAI.Models
{
  public class BacktestResult
  {
    public List<TradeResult> Trades { get; set; } = new();
    public List<EquityPoint> EquityCurve { get; set; } = new();
    public double MaxDrawdown { get; set; }

    // 🧩 New diagnostic fields
    public Dictionary<string, int> SignalCounts { get; set; } = new();
    public Dictionary<string, decimal> PnLByPattern { get; set; } = new();
    public Dictionary<string, int> DirectionCounts { get; set; } = new();
  }





  public class EquityPoint
  {
    public DateTime Time { get; set; }
    public double Balance { get; set; }
    public double Equity { get; set; }
  }
}

public static class BacktestEngine
{
  public static BacktestResult Run(List<Candle> candles, DateTime start, DateTime end, StrategyConfig config)
  {
    var result = new BacktestResult();
    TradeResult? activeTrade = null;
    decimal balance = 0;
    TradeLimiter.Reset();
    var lastOutput = DateTime.MinValue;

    // 🧩 Cache signals once

    var rawSignals = SignalService.ExtractSignals(candles, config);
    DebugLog.WriteLine($"Extracted {rawSignals.Count} signals");

    //var rawSignals = SignalService.ExtractSignals(candles, config)
    //    .Where(s => s.Time >= start && s.Time <= end)
    //    .ToList();

    for (int i = 0; i < candles.Count; i++)
    {
      if ((DateTime.UtcNow - lastOutput).TotalSeconds >= 60)
      {
        DebugLog.WriteLine($"Progress: i = {i}");
        lastOutput = DateTime.UtcNow;
      }

      var candle = candles[i];
      if (candle.Time < start || candle.Time > end)
        continue;

      if (activeTrade == null)
      {
        var signalsAtCandle = rawSignals.Where(s => s.Time == candle.Time).ToList();

        var filtered = SignalFilter.HighestConfidence(signalsAtCandle);

        foreach (var signal in filtered)
        {
          if (TradeLimiter.CanTrade(config.MaxTradesPerRun))
          {
            TradeLimiter.RegisterTrade();
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
      }
      else
      {
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
          TradeLimiter.DeregisterTrade();
        }
      }

      result.EquityCurve.Add(new EquityPoint
      {
        Time = candle.Time,
        Balance = (double)balance,
        Equity = (double)balance // updated at end
      });
    }

    // 🧩 Final equity + drawdown + diagnostics
    decimal equity = 0;
    decimal peak = 0;
    decimal maxDrawdown = 0;

    var signalCounts = new Dictionary<string, int>();
    var pnlByPattern = new Dictionary<string, List<decimal>>();
    var directionCounts = new Dictionary<string, int>();

    foreach (var trade in result.Trades)
    {
      var key = trade.Signal;
      signalCounts[key] = signalCounts.GetValueOrDefault(key) + 1;
      pnlByPattern.TryAdd(key, new List<decimal>());
      pnlByPattern[key].Add(trade.NetPnL);

      var dir = trade.Direction.ToString();
      directionCounts[dir] = directionCounts.GetValueOrDefault(dir) + 1;

      equity += trade.NetPnL;
      peak = Math.Max(peak, equity);
      maxDrawdown = Math.Max(maxDrawdown, peak - equity);
    }

    result.MaxDrawdown = (double)maxDrawdown;
    result.SignalCounts = signalCounts;
    result.PnLByPattern = pnlByPattern.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Sum());
    result.DirectionCounts = directionCounts;

    return result;
  }
}
