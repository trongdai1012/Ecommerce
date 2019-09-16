using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KLTN.Common.Datatables;
using KLTN.Services;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Web.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
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
    }
}