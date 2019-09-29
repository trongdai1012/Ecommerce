namespace KLTN.DataAccess.Models
{
    public class ConfirmForgot
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ConfirmString { get; set; }
    }
}
