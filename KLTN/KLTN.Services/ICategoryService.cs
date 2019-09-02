using KLTN.Common.Datatables;
using KLTN.DataModels.Models.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.Services
{
    public interface ICategoryService
    {
        Tuple<IEnumerable<CategoryViewModel>, int, int> LoadCategory(DTParameters dtParameters);

        CategoryViewModel GetCategoryById(int? id);

        int CreateCategory(CreateCategoryModel model);
    }
}
