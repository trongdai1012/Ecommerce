using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataAccess.Models
{
    public class Image
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Url { get; set; }
        public byte Order { get; set; }

        public Product Product { get; set; }
    }
}
