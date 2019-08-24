using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataModels.Models.Users
{
    public class AuthenticationViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public byte Role { get; set; }
        public string RedirectUrl { get; set; }
    }
}
