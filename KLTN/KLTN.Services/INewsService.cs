using KLTN.Common.Datatables;
using KLTN.DataAccess.Models;
using KLTN.DataModels.Models.News;
using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.Services
{
    public interface INewsService
    {
        Tuple<IEnumerable<NewsViewModel>, int, int> LoadNews(DTParameters dtParameters);

        Tuple<NewsViewModel, int> GetNewsById(int? id);

        int Create(NewsViewModel model);
    }
}
