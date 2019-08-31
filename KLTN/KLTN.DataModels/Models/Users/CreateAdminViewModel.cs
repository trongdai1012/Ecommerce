using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KLTN.DataModels.Models.Users
{
    public class CreateAdminViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string PassEmail { get; set; }
        public string ConfirmPassEmail { get; set; }
        public string FirstName { get; set; }
        public byte Role { get; set; }
        public string LastName { get; set; }
        public bool Gender { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthDay { get; set; }  
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}
