    using System.Collections.Generic;

namespace KLTN.DataAccess.Models
{
    public class Product : BaseModel
    {
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public decimal InitialPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal PromotionPrice { get; set; }
        public int DurationWarranty { get; set; }
        public string MetaTitle { get; set; }
        public string Description { get; set; }
        public float Rate { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { set; get; }
        public int TotalSold { get; set; }
        public int Quantity { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }
        public virtual List<Image> Images { get; set; }
        public virtual List<Feedback> Feedbacks { get; set; }
    }
}
