using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KLTN.Common.Datatables;
using KLTN.DataModels.Models.Products;
using KLTN.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KLTN.Web.Areas.Admin.Controllers
{
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
    }
}