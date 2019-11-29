using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataModels.Models.Products
{
    public class ListLapInModel
    {
        public string StringKey { get; set; }
        public int BrandId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
