namespace KLTN.DataAccess.Models
{
    public class User : BaseModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public byte Role { get; set; }
        public int StoreId { get; set; }
    }
}
