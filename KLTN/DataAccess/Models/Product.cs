namespace DataAccess.Models
{
    class Product : BaseModel
    {
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public decimal Price { get; set; }
        public decimal PromotionPrice { get; set; }
        public int Quantity { get; set; }
        public string MetaTitle { get; set; }
        public string Description { get; set; }
        public byte Rank { get; set; }
        public int ViewCount { get; set; }
    }
}
