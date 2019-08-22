using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataModels.Models.Users
{
    public class UserBaseViewModel
    {
        public int Id { get; set; }
        public byte Role { get; set; }
        public DateTime CreateAt { get; set; }
        public int CreateBy { get; set; }
        public DateTime UpdateAt { get; set; }
        public int UpdateBy { get; set; }
        public bool Status { get; set; }
    }
}
