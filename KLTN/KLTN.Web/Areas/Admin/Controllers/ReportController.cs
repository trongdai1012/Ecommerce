using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KLTN.DataModels.Models.ReportRevenue;
using KLTN.Services;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Web.Areas.Admin.Controllers
{
    public class ReportController : BaseController
    {
        private readonly IOrderService _orderService;

        public ReportController(IOrderService oderService)
        {
            _orderService = oderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Report(FilterDateModel dateModel)
        {
            var report = _orderService.GetReportRevenue(dateModel);
            return PartialView("_ReportRevenue", report);
        }
    }
}