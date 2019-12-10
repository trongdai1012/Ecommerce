using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataModels.Models.Orders
{
    public class OrderDetailViewModel
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
