using KLTN.Common.Datatables;
using KLTN.DataModels.Models.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.Services
{
    public interface IProductService
    {
        Tuple<ProductViewModel, int> GetProductById(int? id);

        Tuple<IEnumerable<LaptopViewModel>, int, int> LoadLaptop(DTParameters dtParameters);

        Tuple<LaptopViewModel, int> GetLaptopById(int? id);

        Tuple<IEnumerable<LaptopViewModel>, IEnumerable<LaptopViewModel>, IEnumerable<LaptopViewModel>, int>
            GetProductRecommender();

        //int CreateCategory(CreateCategoryModel model);

        //IEnumerable<CategoryViewModel> GetAll();
    }
}
