using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataAccess.Models
{
    public class UserRatingProduct
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public byte Rate { get; set; }
        public string Comment { get; set; }
    }
}
