using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataAccess.Models
{
    public class WareHouse : BaseModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        
        public virtual List<ProductWareHoure> ProductWareHoures { get; set; }
    }
}
