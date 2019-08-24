using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataModels.Models.Users
{
    public class UpdateEmployeeViewModel
    {
        public int Id { get; set; }
        public byte Role { get; set; }
        public int StoreId { get; set; }
    }
}
