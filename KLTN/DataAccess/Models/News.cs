using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataAccess.Models
{
    public class News : BaseModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
