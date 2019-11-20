using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KLTN.Common.Datatables;
using KLTN.DataAccess.Models;
using KLTN.DataModels.Models.News;
using KLTN.Services;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Web.Areas.Admin.Controllers
{
    public class NewsController : BaseController
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadNews([FromBody] DTParameters dtParameters)
        {
            var tupleData = _newsService.LoadNews(dtParameters);

            return Json(new
            {
                draw = dtParameters.Draw,
                recordsTotal = tupleData.Item3,
                recordsFiltered = tupleData.Item2,
                data = tupleData.Item1
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
            });
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(NewsViewModel newsModel)
        {
            if (!ModelState.IsValid) return View(newsModel);
            var result = _newsService.Create(newsModel);
            switch (result)
            {
                case 0:
                    ModelState.AddModelError("", "Tiêu đề đã tồn tại");
                    return View(newsModel);
                case 1:
                    return RedirectToAction("Index", "News");
                default:
                    ModelState.AddModelError("", "Có lỗi không xác định khi tạo bài viết, vui lòng liên hệ người quản trị");
                    return View(newsModel);
            }
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            var model = _newsService.GetNewsById(id);

            return View(model.Item1);
        }
    }
}