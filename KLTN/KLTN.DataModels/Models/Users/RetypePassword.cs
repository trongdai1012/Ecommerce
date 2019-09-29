using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataModels.Models.Users
{
    public class RetypePassword
    {
        public string ConfirmString { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
