using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KLTN.Common.Datatables;
using KLTN.Services;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Web.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult OrderDetail(int? id)
        {
            if (id == null) return BadRequest();
            var order = _orderService.GetOrderDetailById(id.Value);
            if (order.Item4 == -1) return BadRequest();
            return View(order);
        }

        [HttpPost]
        public IActionResult LoadOrder([FromBody] DTParameters dtParameters)
        {
            var tupleData = _orderService.LoadOrder(dtParameters);

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
    }
}