using System;
using System.Collections.Generic;

namespace KLTN.DataAccess.Models
{
    public class Order : BaseModel
    {
        public decimal TotalPrice { get; set; }
        public int RecipientProvinceCode { get; set; }
        public int RecipientDistrictCode { get; set; }
        public int RecipientPrecinctCode { get; set; }
        public string RecipientProvinceName { get; set; }
        public string RecipientDistrictName { get; set; }
        public string RecipientPrecinctName { get; set; }
        public string RecipientAddress { get; set; }
        public string RecipientPhone { get; set; }
        public string RecipientName { get; set; }
        public byte StatusOrder { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Delivery Delivery { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }
    }
}
