using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataModels.Models.ReportRevenue
{
    public class OrderChartModel
    {
        public string Date { get; set; }
        public int TotalOrder { get; set; }
        public int TotalOrderFinish { get; set; }
    }
}
