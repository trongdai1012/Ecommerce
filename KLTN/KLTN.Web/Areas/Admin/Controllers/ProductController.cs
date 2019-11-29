using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KLTN.Common.Datatables;
using KLTN.DataModels.Models.Products;
using KLTN.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KLTN.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class ProductController : BaseController
    {
        private readonly IBrandService _brandService;
        private readonly IProductService _productService;

        public ProductController(IProductService productService, IBrandService brandService)
        {
            _brandService = brandService;
            _productService = productService;
        }

        public IActionResult Laptop()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadLaptop([FromBody] DTParameters dtParameters)
        {
            var tupleData = _productService.LoadLaptop(dtParameters);

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

        public IActionResult Mobile()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadMobile([FromBody] DTParameters dtParameters)
        {
            var tupleData = _productService.LoadMobile(dtParameters);

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

        [HttpGet]
        public IActionResult CreateLaptop()
        {
            ViewBag.BrandId = new SelectList(_brandService.GetAll(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateLaptop(CreateLaptopViewModel model, IFormFile imageFileMajor, List<IFormFile> imageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.BrandId = new SelectList(_brandService.GetAll(), "Id", "Name");
                return View(model);
            };
            await _productService.CreateLaptop(model,imageFileMajor,imageFile);
            return RedirectToAction("Laptop","Product");
        }

        [HttpGet]
        public IActionResult CreateMobile()
        {
            ViewBag.BrandId = new SelectList(_brandService.GetAll(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMobile(CreateMoblieViewModel model, IFormFile imageFileMajor, List<IFormFile> imageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.BrandId = new SelectList(_brandService.GetAll(), "Id", "Name");
                return View(model);
            };
            await _productService.CreateMobile(model, imageFileMajor, imageFile);
            return RedirectToAction("Mobile", "Product");
        }

        [HttpGet]
        public IActionResult UpdateLaptop(int id)
        {
            var lap = _productService.GetLaptopUpdateById(id);
            ViewBag.BrandId = new SelectList(_brandService.GetAll(), "Id", "Name",lap.BrandId);
            return View(lap);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLaptop(UpdateLaptopViewModel model, IFormFile imageFileMajor, List<IFormFile> imageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.BrandId = new SelectList(_brandService.GetAll(), "Id", "Name",model.BrandId);
                return View(model);
            };
            await _productService.UpdateLaptop(model, imageFileMajor, imageFile);
            return RedirectToAction("Laptop", "Product");
        }

        [HttpGet]
        public IActionResult UpdateMobile(int id)
        {
            var lap = _productService.GetMobileUpdateById(id);
            ViewBag.BrandId = new SelectList(_brandService.GetAll(), "Id", "Name", lap.BrandId);
            return View(lap);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMobile(UpdateMoblieViewModel model, IFormFile imageFileMajor, List<IFormFile> imageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.BrandId = new SelectList(_brandService.GetAll(), "Id", "Name", model.BrandId);
                return View(model);
            };
            await _productService.UpdateMobile(model, imageFileMajor, imageFile);
            return RedirectToAction("Mobile", "Product");
        }

        /// <summary>
        /// Action ChangeStatus return JsonResult to ajax
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var statusResult = _productService.ChangeStatus(id);

            return Json(new
            {
                status = statusResult
            });
        }
    }
}