using KLTN.Common.Datatables;
using KLTN.DataModels.Models.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.Services
{
    public interface IProductService
    {
        Tuple<IEnumerable<LaptopViewModel>, int, int> LoadLaptop(DTParameters dtParameters);

        Tuple<IEnumerable<LaptopViewModel>, int> GetLaptopTopView();

        Tuple<IEnumerable<LaptopViewModel>, int> GetLaptopTopLike();

        Tuple<IEnumerable<LaptopViewModel>, int> GetLaptopTopSold();

        Tuple<LaptopViewModel, int> GetLaptopById(int? id);

        //int CreateCategory(CreateCategoryModel model);

        //IEnumerable<CategoryViewModel> GetAll();
    }
}
