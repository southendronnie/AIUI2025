public class StrategyConfig
{
    public int StopLossPips { get; set; }
    public int TakeProfitPips { get; set; }
    public decimal PipSize { get; set; }
    public string Timeframe { get; set; } // e.g. "1m", "5m", "1h"
    public decimal SpreadCost { get; set; } // in pips
    public decimal CommissionPerTrade { get; set; } // fixed cost
    public decimal SlippagePips { get; set; } // optional realism
}
