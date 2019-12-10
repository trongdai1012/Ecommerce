using KLTN.Common.Datatables;
using KLTN.DataModels.Models.Brands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KLTN.Services
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandViewModel>> GetAllBrand();

        Tuple<IEnumerable<BrandViewModel>, int, int> LoadBrand(DTParameters dtParameters);

        Tuple<BrandViewModel, int> GetBrandById(int? id);

        int CreateBrand(CreateBrandModel model);

        IEnumerable<BrandViewModel> GetAll();

        bool Update(BrandViewModel model);

        bool ChangeStatus(int id);

        Task<IEnumerable<BrandViewModel>> GetBrandHasLaptop();

        Task<IEnumerable<BrandViewModel>> GetBrandHasMobile();
    }
}
