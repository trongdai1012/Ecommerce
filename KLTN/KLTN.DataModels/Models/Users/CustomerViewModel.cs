using System;

namespace KLTN.DataModels.Models.Users
{
    public class CustomerViewModel : UserBaseViewModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Gender { get; set; }
        public DateTime BirthDay { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public byte Rank { get; set; }
    }
}
