using KLTN.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataModels.Models.Products
{
    public class CartItem
    {
        public ProductViewModel Product { get; set; }

        public int Quantity { get; set; }
    }
}
