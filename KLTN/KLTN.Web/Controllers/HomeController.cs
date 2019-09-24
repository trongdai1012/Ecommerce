using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using KLTN.Web.Models;
using KLTN.Services;

namespace KLTN.Web.Controllers
{
    [Route("Home")]
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        [Route("")]
        [Route("index")]
        [Route("~/")]
        public IActionResult Index()
        {

            return View();
        }

        [Route("ListProduct")]
        [HttpPost]
        public IActionResult ListProduct()
         {
            var model = _productService.GetProductRecommender();
            return PartialView("_RecommenderProduct", model);
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
