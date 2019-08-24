using System;

namespace KLTN.DataAccess.Models
{
    public class BaseModel
    {
        public int Id { get; set; }
        public DateTime CreateAt { get; set; }
        public int CreateBy { get; set; }
        public DateTime UpdateAt { get; set; }
        public int UpdateBy { get; set; }
        public bool Status { get; set; }
    }
}
