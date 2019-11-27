using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KLTN.Common.Datatables;
using KLTN.DataAccess.Models;
using KLTN.DataModels.Models.News;
using KLTN.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

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
        public async Task<IActionResult> Create(NewsViewModel newsModel,IFormFile file)
        {
            if (!ModelState.IsValid) return View(newsModel);
            var result = await _newsService.Create(newsModel, file);
            switch (result)
            {
                case 0:
                    ModelState.AddModelError("", "Tiêu đề đã tồn tại");
                    return View(newsModel);
                case 1:
                    return RedirectToAction("Index", "News");
                case 2:
                    ModelState.AddModelError("", "Ảnh không hợp lệ.");
                    return View(newsModel);
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

        [HttpGet]
        public IActionResult Update(int id)
        {
            try
            {
                var model = _newsService.GetNewsById(id);
                return model.Item1 == null ? BadRequest() : (IActionResult)View(model.Item1);
            }
            catch (Exception e)
            {
                Log.Error("Have an error when update News in Controller", e);
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult Update(NewsViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var result = _newsService.Update(model);

                return result ? (IActionResult)RedirectToAction("Index", "News") : BadRequest();
            }
            catch (Exception e)
            {
                Log.Error("Have an error when update News in Controller", e);
                return BadRequest();
            }
        }

        /// <summary>
        /// Action ChangeStatus return JsonResult to ajax
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var statusResult = _newsService.ChangeStatus(id);

            return Json(new
            {
                status = statusResult
            });
        }
    }
}