using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [DataType(DataType.Date)]
        public DateTime BirthDay { get; set; }
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int PrecinctId { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string PrecinctName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool IsConfirm { get; set; }

        public virtual List<Product> Products { get; set; }
        public virtual List<Brand> Brands { get; set; }
        public virtual List<Contact> Contacts { get; set; }
        public virtual UserConfirm UserConfirm { get; set; }
        public virtual List<Feedback> Feedbacks { get; set; }
    }
}
