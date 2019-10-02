using System;

namespace KLTN.DataModels.Models.Contact
{
    public class ContactModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string CreateBy { get; set; }
        public int HandlerId { get; set; }
        public string ContentReply { get; set; }
        public DateTime ContactAt { get; set; }
        public DateTime ReplyContactAt { get; set; }
        public bool Status { get; set; }

    }
}
