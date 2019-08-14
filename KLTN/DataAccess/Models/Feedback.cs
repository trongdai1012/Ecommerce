using System;

namespace DataAccess.Models
{
    class Feedback
    {
        public int Id { get; set; }
        public int HandlerId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ContentReply { get; set; }
        public DateTime FeedbackAt { get; set; }
        public DateTime ReplyFeedbackAt { get; set; }
        public bool Status { get; set; }
    }
}
