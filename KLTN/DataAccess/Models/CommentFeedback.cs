using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataAccess.Models
{
    public class CommentFeedback : BaseModel
    {
        public int FeedbackId { get; set; }
        public string Comment { get; set; }
    }
}
