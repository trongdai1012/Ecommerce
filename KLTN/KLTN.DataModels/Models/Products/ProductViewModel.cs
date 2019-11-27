using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataModels.Models.Products
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public decimal InitialPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal PromotionPrice { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
    }
}
