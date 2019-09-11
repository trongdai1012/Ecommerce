using KLTN.DataAccess.Models;
using System;
using System.Collections.Generic;

namespace KLTN.DataModels.Models.Products
{
    public class MobileViewModel
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public decimal InitialPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal PromotionPrice { get; set; }
        public int DurationWarranty { get; set; }
        public string MetaTitle { get; set; }
        public string Description { get; set; }
        public List<Image> Images { get; set; }
        public byte Rate { get; set; }
        public List<CommentProduct> Comment { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { set; get; }
        public int TotalSold { get; set; }
        public int Amount { get; set; }
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
        public DateTime CreateAt { get; set; }
        public string CreateBy { get; set; }
        public DateTime UpdateAt { get; set; }
        public string UpdateBy { get; set; }
        public bool Status { get; set; }
    }
}
