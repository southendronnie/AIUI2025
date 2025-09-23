namespace FXAI.Models;

public class StrategyConfig
{
    public int StopLossPips { get; set; } = 10;
    public int TakeProfitPips { get; set; } = 20;
    public int HoldingPeriodMinutes { get; set; } = 2;
    public double PipSize { get; set; } = 0.0001;
}
