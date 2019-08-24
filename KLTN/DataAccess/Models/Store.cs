using System.Collections.Generic;

namespace KLTN.DataAccess.Models
{
    public class Store : BaseModel
    {
        public int WareHouseId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public virtual WareHouse WareHouse { get; set; }
        public virtual List<Employee> Employees { get; set; }
    }
}
