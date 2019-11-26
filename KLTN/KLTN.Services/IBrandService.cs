using KLTN.Common.Datatables;
using KLTN.DataModels.Models.Brands;
using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.Services
{
    public interface IBrandService
    {
        Tuple<IEnumerable<BrandViewModel>, int, int> LoadBrand(DTParameters dtParameters);

        Tuple<BrandViewModel, int> GetBrandById(int? id);

        int CreateBrand(CreateBrandModel model);

        IEnumerable<BrandViewModel> GetAll();

        bool Update(BrandViewModel model);

        bool ChangeStatus(int id);
    }
}
