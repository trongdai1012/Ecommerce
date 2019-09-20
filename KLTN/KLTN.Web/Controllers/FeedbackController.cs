using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KLTN.Services;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Web.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpGet]
        public IActionResult Rating(int productId, string comment, byte rate)
        {
            _feedbackService.Rating(productId, comment, rate);

            return RedirectToAction("Index","Cart");
        }
    }
}