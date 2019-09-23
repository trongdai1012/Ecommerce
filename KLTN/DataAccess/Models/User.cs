using System;
using System.Collections.Generic;

namespace KLTN.DataAccess.Models
{
    public class User : BaseModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public byte Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Gender { get; set; }
        public DateTime BirthDay { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool IsConfirm { get; set; }

        public virtual List<Brand> Brands { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Admin Admin { get; set; }
        public virtual UserConfirm UserConfirm { get; set; }
        public virtual List<Feedback> Feedbacks { get; set; }
    }
}
