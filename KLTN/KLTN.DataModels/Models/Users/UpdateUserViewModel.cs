using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KLTN.DataModels.Models.Users
{
    public class UpdateUserViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int PrecinctId { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string PrecinctName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool Gender { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthDay { get; set; }
    }
}
