using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataModels.Models.News
{
    public class NewsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreateAt { get; set; }
        public string CreateBy { get; set; }
        public DateTime UpdateAt { get; set; }
        public string UpdateBy { get; set; }
        public string Image { get; set; }
        public bool Status { get; set; }
    }
}
