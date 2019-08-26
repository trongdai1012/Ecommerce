using System;

namespace KLTN.DataAccess.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Gmail { get; set; }
        public string PassGmail { get; set; }
        public int StoreId { get; set; }

        public User User { get; set; }
    }
}
