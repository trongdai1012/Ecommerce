using System;
using System.Collections.Generic;

namespace KLTN.DataAccess.Models
{
    public class Customer : BaseModel
    {
        public int UserId { get; set; }
        public byte Rank { get; set; }

        public User User { get; set; }
    }
}
