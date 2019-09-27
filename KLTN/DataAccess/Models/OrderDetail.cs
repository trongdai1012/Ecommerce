using System;

namespace KLTN.DataAccess.Models
{
    public class OrderDetail
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ImageProduct { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
