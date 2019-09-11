using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataAccess.Models
{
    public class CommentProduct : BaseModel
    {
        public int ProductId { get; set; }
        public int Comment { get; set; }
    }
}
