using KLTN.DataModels.Models.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.Services
{
    public interface IRecommenderService
    {
        IEnumerable<LaptopViewModel> RecommenderProduct();
    }
}
