using KLTN.DataAccess.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace KLTN.DataModels.Models.Brands
{
    public class CreateBrandModel
    {
        public CreateBrandModel()
        {
        }

        public string Name { get; set; }
        public string Address { get; set; }

        //public List<SelectListItem> ListCategory { get; set; }
    }
}
