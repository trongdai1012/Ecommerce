using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataModels.Models.ReportRevenue
{
    public class ReportRevenueModel
    {
        public string Date { get; set; }
        public int TotalOrder { get; set; }
        public int TotalOrderFinish { get; set; }
        public int TotalOrderCancel { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
