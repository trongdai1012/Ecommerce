using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataModels.Models.Category
{
    public class CreateCategoryModel
    {
        public string Name { get; set; }
        public int ParrentCategoryID { get; set; }
    }
}
