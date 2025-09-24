using System;

namespace FXAI.Models;

public class SimulatedTrade
{
    public DateTime EntryTime { get; set; }
    public double EntryPrice { get; set; }
    public DateTime ExitTime { get; set; }
    public double ExitPrice { get; set; }
    public string PatternType { get; set; } = string.Empty;
    public double Profit => ExitPrice - EntryPrice;
}
