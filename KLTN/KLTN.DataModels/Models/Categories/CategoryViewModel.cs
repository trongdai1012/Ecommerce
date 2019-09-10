using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataModels.Models.Categories
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public bool Status { get; set; }
    }
}
