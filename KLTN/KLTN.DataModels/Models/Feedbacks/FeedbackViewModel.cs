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
        public byte Rate { get; set; }
        public string Comment { get; set; }
        public bool IsLike { get; set; }
        public bool IsBought { get; set; }
    }
}
