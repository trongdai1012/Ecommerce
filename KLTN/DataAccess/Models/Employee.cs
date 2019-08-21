using System;

namespace KLTN.DataAccess.Models
{
    public class Employee : BaseModel
    {
        public int UserId { get; set; }
        public string Gmail { get; set; }
        public string PassGmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Gender { get; set; }
        public DateTime BirthDay { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int StoreId { get; set; }

        public User User { get; set; }
    }
}
