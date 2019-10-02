using System;

namespace KLTN.DataAccess.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public int CreateBy { get; set; }
        public int HandlerId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ContentReply { get; set; }
        public DateTime ContactAt { get; set; }
        public DateTime ReplyContactAt { get; set; }
        public bool Status { get; set; }

        public virtual User User { get; set; }
    }
}
