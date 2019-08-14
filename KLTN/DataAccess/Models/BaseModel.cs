using System;

namespace DataAccess.Models
{
    class BaseModel
    {
        public int Id { get; set; }
        public DateTime CreateAt { get; set; }
        public string CreateBy { get; set; }
        public DateTime UpdateAt { get; set; }
        public string UpdateBy { get; set; }
        public bool Status { get; set; }
    }
}
