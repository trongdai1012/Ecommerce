namespace KLTN.DataAccess.Models
{
    public class Mobile
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Screen { get; set; }
        public string OperatingSystem { get; set; }
        public string RearCamera { get; set; }
        public string FrontCamera { get; set; }
        public string CPU { get; set; }
        public string RAM { get; set; }
        public string ROM { get; set; }
        public string SIM { get; set; }
        public string Pin { get; set; }
        public string Color { get; set; }
        public int BrandId { get; set; }

        public virtual Brand Brand { get; set; }
    }
}
