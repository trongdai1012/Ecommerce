using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataAccess.Models
{
    public class UserConfirm
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ConfirmString { get; set; }

        public virtual User User { get; set; }
    }
}
