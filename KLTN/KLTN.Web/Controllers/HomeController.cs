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

        private readonly IRecommenderService _recommenderService;

        public HomeController(IProductService productService, IRecommenderService recommenderService)
        {
            _recommenderService = recommenderService;
            _productService = productService;
        }

        [Route("")]
        [Route("index")]
        [Route("~/")]
        public IActionResult Index()
        {
            var a = _recommenderService.GetAllLaptop();
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
