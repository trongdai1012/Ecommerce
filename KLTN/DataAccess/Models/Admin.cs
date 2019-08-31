using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataAccess.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PassEmail { get; set; }
        public bool IsMaster { get; set; }

        public virtual User User { get; set; }
    }
}
