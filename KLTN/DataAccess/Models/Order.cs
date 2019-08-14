using System;

namespace DataAccess.Models
{
    class Order : BaseModel
    {
        public decimal TotalPrice { get; set; }
        public string AddressRecipient { get; set; }
        public string PhoneRecipient { get; set; }
        public string NameRecipient { get; set; }
    }
}
