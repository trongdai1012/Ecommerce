using System;
using System.Linq;
using KLTN.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using X.PagedList;

namespace KLTN.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        private readonly IFeedbackService _feedbackService;

        public ProductController(IProductService productService, IFeedbackService feedbackService)
        {
            _productService = productService;

            _feedbackService = feedbackService;
        }

        public IActionResult LaptopDetail(int id)
        {
            var laptop = _productService.GetLaptopById(id).Item1;
            return View(laptop);
        }

        public IActionResult MobileDetail(int id)
        {
            var mobile = _productService.GetMobileById(id).Item1;
            return View(mobile);
        }

        [HttpPost]
        public JsonResult Rating(int productId, byte rate, string comment)
        {
            if(rate<1 || rate > 5) return Json( new {status = 3});
            try
            {
                var myRating = _feedbackService.Rating(productId, comment, rate);
                return Json(
                    new
                    {
                        status = myRating
                    });
            }
            catch (Exception e)
            {
                Log.Error("Have an error when Rating product in controller", e);
                return Json(
                    new
                    {
                        status = false
                    });
            }
        }

        [HttpPost]
        public JsonResult LikeProduct(int productId)
        {
            var feedback = _feedbackService.LikeProduct(productId);
            return Json(
                new
                {
                    Feedback = feedback
                });
        }
        
        [HttpPost]
        public IActionResult FeedbackProduct(int id)
        {
            try
            {
                var listFeedback = _feedbackService.GetFeedbackByProducId(id);
                return PartialView("_FeedbackProduct", listFeedback);
            }
            catch (Exception e)
            {
                Log.Error("Have an error at FeedbackProduct in ProductController", e);
                return BadRequest();
            }
        }

        public IActionResult LapTop(string searchKey, int pageIndex = 1, int pageSize = 12)
        {
            var listLap = _productService.GetAllLaptop(searchKey);

            return View(listLap.OrderBy(x=>x.InitialPrice).ToPagedList(pageIndex, pageSize));
        }

        public IActionResult Mobile(string searchKey, int pageIndex = 1, int pageSize = 12)
        {
            var listMobile = _productService.GetAllMobile(searchKey);

            return View(listMobile.OrderBy(x => x.InitialPrice).ToPagedList(pageIndex, pageSize));
        }
    }
}