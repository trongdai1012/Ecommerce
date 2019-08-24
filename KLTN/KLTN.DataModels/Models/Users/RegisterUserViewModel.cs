using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KLTN.DataModels.Models.Users
{
    public class RegisterUserViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Gender { get; set; }
        public DateTime BirthDay { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}
