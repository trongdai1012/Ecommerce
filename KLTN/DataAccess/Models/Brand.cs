using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataAccess.Models
{
    public class Brand : BaseModel
    {
        public string Name { get; set; }
        public string Address { get; set; }

        public virtual User User { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}
