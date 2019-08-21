using System.Collections.Generic;

namespace KLTN.DataAccess.Models
{
    public class Product : BaseModel
    {
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public decimal Price { get; set; }
        public decimal PromotionPrice { get; set; }
        public string MetaTitle { get; set; }
        public string Description { get; set; }
        public byte Rate { get; set; }
        public int ViewCount { get; set; }

        public virtual List<ProductWareHoure> ProductWareHoures { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }
        public virtual Category Category { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}
