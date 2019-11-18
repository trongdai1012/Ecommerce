using KLTN.DataAccess.Models;
using System;
using System.Collections.Generic;

namespace KLTN.DataModels.Models.Products
{
    public class UpdateLaptopViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BrandId { get; set; }
        public decimal InitialPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal PromotionPrice { get; set; }
        public int DurationWarranty { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
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
    }
}
