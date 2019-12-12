using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using KLTN.Web.Models;
using KLTN.Services;
using System;
using KLTN.DataModels.Models.Products;
using System.Collections.Generic;

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
            _recommenderService.RecommenderProduct();
            var model = _productService.GetBanner();
            return View(model);
        }

        [Route("ListProduct")]
        [HttpPost]
        public IActionResult ListProduct()
        {
            var model = _productService.GetProductRecommender();

            var matrixFacto = _recommenderService.RecommenderProduct();

            var allModel = new Tuple<IEnumerable<LaptopViewModel>, IEnumerable<LaptopViewModel>, IEnumerable<LaptopViewModel>,
                IEnumerable<LaptopViewModel>>(model.Item1, model.Item2, model.Item3, matrixFacto);

            return PartialView("_RecommenderProduct", allModel);
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
