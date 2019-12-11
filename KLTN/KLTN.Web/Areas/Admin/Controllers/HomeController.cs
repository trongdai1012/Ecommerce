﻿using KLTN.DataModels.Models.ReportRevenue;
using KLTN.Services;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IOrderService _orderService;

        public HomeController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult OrderChart(FilterModel dataModel)
        {
            var order = _orderService.GetOrderChart(dataModel);
            return PartialView("_OrderChart", order);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ProductChart(FilterModel dataModel)
        {
            var listProduct = _orderService.GetTopSoldChart(dataModel);
            return PartialView("_ProductChart", listProduct);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RevenueChart(FilterModel dataModel)
        {
            var listRevenue = _orderService.GetRevenue(dataModel);
            return PartialView("_RevenueChart", listRevenue);
        }

        [HttpPost]
        public IActionResult ReportOverView(FilterModel dataModel)
        {
            var orverView = _orderService.GetReportOverView(dataModel);
            return PartialView("_ReportOverView", orverView);
        }
    }
}