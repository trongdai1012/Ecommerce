using KLTN.DataModels.Models.ReportRevenue;
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
    }
}