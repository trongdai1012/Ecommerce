using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KLTN.Common.Datatables;
using KLTN.DataModels.Models.Category;
using KLTN.Services;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Web.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadCategory([FromBody] DTParameters dtParameters)
        {
            var tupleData = _categoryService.LoadCategory(dtParameters);

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
        public IActionResult Create(CreateCategoryModel categoryModel)
        {
            if (!ModelState.IsValid) return View(categoryModel);
            var result = _categoryService.CreateCategory(categoryModel);
            switch(result)
            {
                case 0:
                    ModelState.AddModelError("", "Tên thể loại sản phẩm đã tồn tại");
                    return View(categoryModel);
                case 1:
                    return RedirectToAction("Index", "Home");
                default:
                    ModelState.AddModelError("", "Có lỗi không xác định khi tạo thể loại sản phẩm, vui lòng liên hệ người quản trị");
                    return View(categoryModel);
            }
        }
    }
}