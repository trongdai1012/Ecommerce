using KLTN.Common.Datatables;
using KLTN.DataAccess.Models;
using KLTN.DataModels.Models.News;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KLTN.Services
{
    public interface INewsService
    {
        Tuple<IEnumerable<NewsViewModel>, int, int> LoadNews(DTParameters dtParameters);

        Tuple<NewsViewModel, int> GetNewsById(int? id);

        Task<int> Create(NewsViewModel model, IFormFile image);

        bool Update(NewsViewModel viewModel);

        bool ChangeStatus(int id);

        IEnumerable<NewsViewModel> GetAll();

        IEnumerable<NewsViewModel> GetSixNews(int id);
    }
}
