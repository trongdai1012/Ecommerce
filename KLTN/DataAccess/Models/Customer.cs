using System;

namespace DataAccess.Models
{
    class Customer : BaseModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Gender { get; set; }
        public DateTime BirthDay { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public byte Rank { get; set; }
        public string FbAddress { get; set; }
        public string GmailAddress { get; set; }
        public string ZaloAddress { get; set; }
    }
}
