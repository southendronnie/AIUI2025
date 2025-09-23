using System;

namespace FXAI.Models;

public class BacktestRequest
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public int StopLossPips { get; set; } = 10;
    public int TakeProfitPips { get; set; } = 20;
    public int HoldingPeriodMinutes { get; set; } = 2;
}
