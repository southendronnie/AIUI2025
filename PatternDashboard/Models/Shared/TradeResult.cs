using FXAI.Core.Services;

public class TradeResult
{
  internal object Instrument;

  public string OrderFillTransaction { get; set; }
  public DateTime EntryTime { get; set; }
  public DateTime ExitTime { get; set; }
  public decimal EntryPrice { get; set; }
  public decimal ExitPrice { get; set; }
  public decimal RawPnL { get; set; }
  public decimal NetPnL { get; set; }
  public decimal Cost { get; set; }
  public decimal SL { get; set; }                 // Stop loss in pips
  public decimal TP { get; set; }                 // Take profit in pips
  public PatternType Pattern { get; set; }
  // 🔍 Execution diagnostics
  public decimal SpreadCost { get; set; }
  public decimal SlippageCost { get; set; }

  // 🧠 Signal metadata
  public string Signal { get; set; } = string.Empty;
  public string Direction { get; set; } = string.Empty;
  public decimal BrokerPnL { get; set; } // for reconciliation

  public bool IsWin { get; set; }
  public decimal PnL { get; set; } // in pips
  public bool IsClosed { get; set; } = false;
  public bool IsDryRun { get; set; } = true;

  public string BrokerId { get; set; }
  public decimal FillPrice { get; set; }
  public DateTime ExecutedAt { get; set; }
  public DateTime TimeOpened { get; set; }
  public DateTime TimeClosed { get; set; }
  public double Confidence { get; set; } // from signal
  public string SignalTimeFrame { get; set; } = "M15"; // optional


}