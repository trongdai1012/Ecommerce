namespace KLTN.DataAccess.Models
{
    public class ProductWareHoure
    {
        public int ProductQuantity { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int WareHouseId { get; set; }
        public virtual WareHouse WareHouse { get; set; }
    }
}
