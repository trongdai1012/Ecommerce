using System;
using System.Collections.Generic;

namespace KLTN.DataAccess.Models
{
    public class Customer : BaseModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Gender { get; set; }
        public DateTime BirthDay { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public byte Rank { get; set; }

        public User User { get; set; }
        public virtual List<Feedback> Feedbacks { get; set; }
    }
}
