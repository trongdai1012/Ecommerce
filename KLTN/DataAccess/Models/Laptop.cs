using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataAccess.Models
{
    public class Laptop
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Screen { get; set; }
        public string OperatingSystem { get; set; }
        public string Camera { get; set; }
        public string CPU { get; set; }
        public string RAM { get; set; }
        public string ROM { get; set; }
        public string Card { get; set; }
        public string Design { get; set; }
        public string Size { get; set; }
        public string PortSupport { get; set; }
        public string Pin { get; set; }
        public string Color { get; set; }
        public string Weight { get; set; }

        public Product Product { get; set; }
    }
}
