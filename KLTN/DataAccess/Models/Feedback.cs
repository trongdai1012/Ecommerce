using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataAccess.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public byte Rate { get; set; }
        public string Comment { get; set; }
        public bool IsLike { get; set; }
        public bool IsBought { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
    }
}
