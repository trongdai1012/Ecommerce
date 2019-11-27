using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KLTN.DataModels.Models.News;
using KLTN.Services;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace KLTN.Web.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        public IActionResult Index(int pageIndex = 1, int pageSize = 12)
        {
            var model = _newsService.GetAll();

            return View(model.OrderBy(x => x.CreateAt).ToPagedList(pageIndex, pageSize));
        }

        public IActionResult Detail(int id)
        {
            var model = _newsService.GetNewsById(id);

            var sixNews = _newsService.GetSixNews(id);

            var combo = new Tuple<NewsViewModel, IEnumerable<NewsViewModel>>(model.Item1, sixNews);

            return View(combo);
        }
    }
}