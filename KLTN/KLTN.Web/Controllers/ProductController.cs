using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KLTN.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;

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

        //public JsonResult Rating(int productId, byte rate, string comment)
        //{
        //    try
        //    {
        //        var myRating = _feedbackService.Rating(productId, comment, rate);
        //        if(myRating)
        //        {
        //            return Json(
        //                new {
                            
        //                });
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //}

        [HttpGet]
        public IActionResult FeedbackProduct(int id)
        {
            try
            {
                var listFeedback = _feedbackService.GetFeedbackByProducId(id);
                return PartialView("FeedbackProduct", listFeedback);
                
            }
            catch(Exception e)
            {
                Log.Error("Have an error at FeedbackProduct in ProductController", e);
                return BadRequest();
            }
        }
    }
}