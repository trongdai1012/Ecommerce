using System.Linq;
using AutoMapper;
using KLTN.Common.Datatables;
using KLTN.DataModels.Models.Brands;
using KLTN.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KLTN.Web.Areas.Admin.Controllers
{
    public class BrandController : BaseController
    {
        private readonly IBrandService _brandService;
        private readonly IMapper _mapper;

        public BrandController(IBrandService brandService, IMapper mapper)
        {
            _brandService = brandService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadBrand([FromBody] DTParameters dtParameters)
        {
            var tupleData = _brandService.LoadBrand(dtParameters);

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
            //var listCateParent = _categoryService.GetAll();
            //var cate = new CreateBrandModel();
            //cate.ListCategory = listCateParent.Select(x =>
            //    new SelectListItem()
            //    {
            //        Text = x.Name,
            //        Value = x.Id.ToString()
            //    }
            //).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateBrandModel brandModel)
        {
            if (!ModelState.IsValid) return View(brandModel);
            var result = _brandService.CreateBrand(brandModel);
            switch(result)
            {
                case 0:
                    ModelState.AddModelError("", "Tên thể loại sản phẩm đã tồn tại");
                    return View(brandModel);
                case 1:
                    return RedirectToAction("Index", "Brand");
                default:
                    ModelState.AddModelError("", "Có lỗi không xác định khi tạo thể loại sản phẩm, vui lòng liên hệ người quản trị");
                    return View(brandModel);
            }
        }
    }
}