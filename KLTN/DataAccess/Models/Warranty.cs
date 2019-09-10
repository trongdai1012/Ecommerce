using System;

namespace KLTN.DataAccess.Models
{
    public class Warranty
    {
        public int Id { get; set; }
        public int ProducId { get; set; }
        public int CustomerId { get; set; }
        public DateTime DateActive { get; set; }
        public DateTime WarrantyPeriod { get; set; }
    }
}
