namespace DataAccess.Models
{
    class Category : BaseModel
    {
        public string Name { get; set; }
        public int ParrentCategoryId { get; set; }
    }
}
