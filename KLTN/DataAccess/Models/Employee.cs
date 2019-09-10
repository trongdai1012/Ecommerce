using System;

namespace KLTN.DataAccess.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PassEmail { get; set; }

        public User User { get; set; }
    }
}
