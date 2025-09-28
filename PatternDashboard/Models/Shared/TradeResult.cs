public class TradeResult
{
  public DateTime EntryTime { get; set; }
  public DateTime ExitTime { get; set; }
  public decimal EntryPrice { get; set; }  // ✅ Add this
  public decimal ExitPrice { get; set; }   // ✅ Add this
  public decimal RawPnL { get; set; }
  public decimal NetPnL { get; set; }
  public decimal Cost { get; set; }
  public decimal SpreadCost { get; set; }
  public decimal SlippageCost { get; set; }
  public string Signal { get; set; } = string.Empty;
  public string Direction { get; set; } = string.Empty;
}