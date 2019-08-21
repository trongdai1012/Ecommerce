using System;
using System.Collections.Generic;

namespace KLTN.DataAccess.Models
{
    public class Order : BaseModel
    {
        public decimal TotalPrice { get; set; }
        public string RecipientAddress { get; set; }
        public string RecipientPhone { get; set; }
        public string RecipientName { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Delivery Delivery { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }
    }
}
