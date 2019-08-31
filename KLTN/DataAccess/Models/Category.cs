using System.Collections.Generic;

namespace KLTN.DataAccess.Models
{
    public class Category : BaseModel
    {
        public string Name { get; set; }
        public int ParrentCategoryId { get; set; }

        public virtual User User { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}
