using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataModels.Models.ReportRevenue
{
    public class FilterDateModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string MonthDate { get; set; }
        public string YearDate { get; set; }
    }
}
