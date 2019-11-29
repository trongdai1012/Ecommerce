using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataAccess.Models
{
    public class BrandHasCate
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
    }
}
