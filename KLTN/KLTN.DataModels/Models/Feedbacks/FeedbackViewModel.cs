using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataModels.Models.Feedbacks
{
    public class FeedbackViewModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public byte Rate { get; set; }
        public string Comment { get; set; }
        public bool IsLike { get; set; }
        public bool IsBought { get; set; }
        public DateTime RatedAt { get; set; }
        public DateTime CreateAt { get; set; }
        public bool Status { get; set; }
    }
}
