using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataModels.Models.Users
{
    public class UpdateEmployeeViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string PassEmail { get; set; }
        public string ConfirmPassEmail { get; set; }
        public byte Role { get; set; }
        public int StoreId { get; set; }
    }
}
