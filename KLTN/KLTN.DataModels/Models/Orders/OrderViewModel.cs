using System;

namespace KLTN.DataModels.Models.Orders
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public int RecipientProvinceCode { get; set; }
        public int RecipientDistrictCode { get; set; }
        public int RecipientPrecinctCode { get; set; }
        public string RecipientAddress { get; set; }
        public string RecipientPhone { get; set; }
        public string RecipientFirstName { get; set; }
        public string RecipientLastName { get; set; }
        public DateTime CreateAt { get; set; }
        public int CreateBy { get; set; }
        public DateTime UpdateAt { get; set; }
        public int UpdateBy { get; set; }
        public bool Status { get; set; }

        public object ToArray()
        {
            throw new NotImplementedException();
        }
    }
}
