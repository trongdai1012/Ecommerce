﻿using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataAccess.Models
{
    public class Admin : BaseModel
    {
        public int UserId { get; set; }
        public string Gmail { get; set; }
        public string PassGmail { get; set; }

        public virtual User User { get; set; }
    }
}
