using System;

namespace KLTN.DataAccess.Models
{
    public class OrderDetail
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreateAt { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
