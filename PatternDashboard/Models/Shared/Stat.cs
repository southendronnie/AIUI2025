using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

  public static class Stat
  {
    public static string? Url { get; set; }
  public static OandaCandleService Oanda { get; internal set; }
  public static DateTime WindowStart { get; set; }
  public static DateTime WindowEnd { get; set; }

}
