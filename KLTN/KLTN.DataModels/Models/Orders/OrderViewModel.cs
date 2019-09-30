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
        public string RecipientProvinceName { get; set; }
        public string RecipientDistrictName { get; set; }
        public string RecipientPrecinctName { get; set; }
        public string RecipientAddress { get; set; }
        public string RecipientPhone { get; set; }
        public string RecipientFirstName { get; set; }
        public string RecipientLastName { get; set; }
        public string RecipientEmail { get; set; }
    }
}
