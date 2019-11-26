using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KLTN.Common.Datatables;
using KLTN.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace KLTN.Web.Areas.Admin.Controllers
{
    public class FeedbackController : BaseController
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadFeedback([FromBody] DTParameters dtParameters)
        {
            var tupleData = _feedbackService.LoadFeedback(dtParameters);

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
        public IActionResult Detail(int id)
        {
            var model = _feedbackService.GetFeedbackById(id);

            return View(model.Item1);
        }

        /// <summary>
        /// Action ChangeStatus return JsonResult to ajax
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var statusResult = _feedbackService.ChangeStatus(id);

            return Json(new
            {
                status = statusResult
            });
        }
    }
}