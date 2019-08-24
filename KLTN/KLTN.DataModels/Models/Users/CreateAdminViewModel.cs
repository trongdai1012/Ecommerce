using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataModels.Models.Users
{
    public class CreateAdminViewModel
    {
        public string Email { get; set; }
        public string PassEmail { get; set; }
        public string ConfirmPassEmail { get; set; }
        public string Gmail { get; set; }
        public string PassGmail { get; set; }
        public string FirstName { get; set; }
        public byte Role { get; set; }
        public string LastName { get; set; }
        public bool Gender { get; set; }
        public DateTime BirthDay { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}
