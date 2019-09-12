using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using KLTN.Web.Models;
using KLTN.Services;

namespace KLTN.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            ViewBag.TopView = _productService.GetLaptopTopView().Item1;

            ViewBag.TopLike = _productService.GetLaptopTopLike().Item1;

            ViewBag.TopSold = _productService.GetLaptopTopSold().Item1;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
