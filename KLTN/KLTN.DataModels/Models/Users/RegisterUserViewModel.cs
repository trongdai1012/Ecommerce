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
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int PrecinctId { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string PrecinctName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}
