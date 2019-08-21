using System;
using System.Collections.Generic;

namespace KLTN.DataAccess.Models
{
    public class Order : BaseModel
    {
        public decimal TotalPrice { get; set; }
        public string AddressRecipient { get; set; }
        public string PhoneRecipient { get; set; }
        public string NameRecipient { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Delivery Delivery { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }
    }
}
