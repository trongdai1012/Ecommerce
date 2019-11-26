using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataAccess.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public byte Rate { get; set; }
        public string Comment { get; set; }
        public bool IsLike { get; set; }
        public bool IsBought { get; set; }
        public DateTime RatedAt { get; set; }
        public DateTime CreateAt { get; set; }
        public bool Status { get; set; }

        public virtual User User { get; set; }
        public virtual Product Product { get; set; }
    }
}
