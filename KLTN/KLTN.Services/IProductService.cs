using KLTN.Common.Datatables;
using KLTN.DataModels.Models.Products;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KLTN.Services
{
    public interface IProductService
    {
        IEnumerable<LaptopViewModel> GetAllLaptop(string key);

        Tuple<ProductViewModel, int> GetProductById(int? id);

        Tuple<IEnumerable<LaptopViewModel>, int, int> LoadLaptop(DTParameters dtParameters);

        Tuple<LaptopViewModel, int> GetLaptopById(int? id);

        Tuple<IEnumerable<LaptopViewModel>, IEnumerable<LaptopViewModel>, IEnumerable<LaptopViewModel>, int>
            GetProductRecommender();

        Task<int> CreateLaptop(CreateLaptopViewModel laptopModel, IFormFile path);

        //int CreateCategory(CreateCategoryModel model);

        //IEnumerable<CategoryViewModel> GetAll();
    }
}
