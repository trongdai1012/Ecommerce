using System;

namespace KLTN.DataAccess.Models
{
    public class Feedback : BaseModel
    {
        public int HandlerId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ContentReply { get; set; }
        public DateTime FeedbackAt { get; set; }
        public DateTime ReplyFeedbackAt { get; set; }

        public virtual User User { get; set; }
    }
}
