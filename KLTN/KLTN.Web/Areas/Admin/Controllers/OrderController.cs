﻿using System;
using System.Linq;
using KLTN.Common.Datatables;
using KLTN.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace KLTN.Web.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;

        public string ErrorMessage { get; set; }

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Manager")]
        public IActionResult OrderWaitConfirm()
        {
            return View();
        }


        [Authorize(Roles = "Admin,Manager,WareHouseStaff")]
        public IActionResult OrderWaitPrepare()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Manager,WareHouseStaff")]
        public IActionResult OrderPreparing()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Manager,Shipper")]
        public IActionResult OrderWaitDelivery()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Manager,Shipper")]
        public IActionResult OrderDelivering()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Manager")]
        public IActionResult OrderFinish()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Manager")]
        public IActionResult OrderCancel()
        {
            return View();
        }

        public IActionResult TaskByUserId()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NotificationMission()
        {
            var model = _orderService.GetMission();
            return PartialView("_GetMission",model);
        }

        [HttpGet]
        public IActionResult OrderDetail(int? id, string errorMessage)
        {
            ViewBag.ErrorMessage = errorMessage;
            if (id == null) return BadRequest();
            var order = _orderService.GetOrderDetailById(id.Value);
            if (order.Item4 == -1) return BadRequest();
            return View(order);
        }

        [Authorize(Roles = "Admin,Manager")]
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

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public IActionResult LoadOrderWaitConfirm([FromBody] DTParameters dtParameters)
        {
            var tupleData = _orderService.LoadOrderWaitConfirm(dtParameters);

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

        [Authorize(Roles = "Admin,Manager,WareHouseStaff")]
        [HttpPost]
        public IActionResult LoadOrderWaitPrepare([FromBody] DTParameters dtParameters)
        {
            var tupleData = _orderService.LoadOrderWaitPrepare(dtParameters);

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

        [Authorize(Roles = "Admin,Manager,WareHouseStaff")]
        [HttpPost]
        public IActionResult LoadOrderPreparing([FromBody] DTParameters dtParameters)
        {
            var tupleData = _orderService.LoadOrderPreparing(dtParameters);

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

        [Authorize(Roles = "Admin,Manager,Shipper")]
        [HttpPost]
        public IActionResult LoadOrderWaitDelivery([FromBody] DTParameters dtParameters)
        {
            var tupleData = _orderService.LoadOrderWaitDelivery(dtParameters);

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

        [Authorize(Roles = "Admin,Manager,Shipper")]
        [HttpPost]
        public IActionResult LoadOrderDelivering([FromBody] DTParameters dtParameters)
        {
            var tupleData = _orderService.LoadOrderDelivering(dtParameters);

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

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public IActionResult LoadOrderFinish([FromBody] DTParameters dtParameters)
        {
            var tupleData = _orderService.LoadOrderFinish(dtParameters);

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

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public IActionResult LoadOrderCancel([FromBody] DTParameters dtParameters)
        {
            var tupleData = _orderService.LoadOrderCancel(dtParameters);

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
        
        [HttpPost]
        public IActionResult LoadTaskByUserId([FromBody] DTParameters dtParameters)
        {
            var tupleData = _orderService.LoadTaskByUserId(dtParameters);

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

        [Authorize(Roles = "Admin,Manager")]
        [HttpGet]
        public IActionResult ConfirmOrder(int id)
        {
            try
            {
                var result = _orderService.OrderConfirm(id);
                if (result == 1) ErrorMessage = "Xác thực đơn hàng thành công";
                if (result == -1) ErrorMessage = "Có lỗi không xác định trong quá trình xác thực đơn hàng, vui lòng liên hệ người quản trị website!";
                if (result == 0) ErrorMessage = "Đơn hàng đã được xác thực bởi một nhân viên khác.";

                return Redirect("~/Admin/Order/Index");
            }
            catch(Exception e)
            {
                Log.Error("Have an error when confirm order in controller", e);
                return BadRequest();
            }
        }

        [Authorize(Roles = "WareHouseStaff")]
        [HttpGet]
        public IActionResult StartPrepareOrder(int id)
        {
            try
            {
                var result = _orderService.StartPrepareOrder(id);
                if (result == 1) ErrorMessage = "Đã chuyển sang trạng thái chuẩn bị đơn hàng";
                if (result == 2) ErrorMessage = "Vui lòng xác thực đơn hàng trước.";
                if (result == 3) ErrorMessage = "Đơn hàng đã được chuẩn bị bởi một nhân viên khác";
                if (result == -1) ErrorMessage = "Có lỗi không xác định trong quá trình xác thực đơn hàng, vui lòng liên hệ người quản trị website!";

                return Redirect("~/Admin/Order/OrderDetail/?id=" + id + "&errorMessage=" + ErrorMessage);
            }
            catch (Exception e)
            {
                Log.Error("Have an error when confirm order in controller", e);
                return BadRequest();
            }
        }

        [Authorize(Roles = "WareHouseStaff")]
        [HttpGet]
        public IActionResult FinishPrepareOrder(int id)
        {
            try
            {
                var result = _orderService.FinishPrepareOrder(id);
                if (result == 1) return Redirect("~/Admin/Order/OrderWaitPrepare"); ;
                if (result == 2) ErrorMessage = "Vui lòng bắt đầu chuẩn bị đơn hàng trước.";
                if (result == 3) ErrorMessage = "Đơn hàng đã được chuẩn bị xong bởi một nhân viên khác.";
                if (result == 4) ErrorMessage = "Chỉ có người chuẩn bị đơn hàng mới có quyền hoàn thành việc chuẩn bị đơn.";
                if (result == -1) ErrorMessage = "Có lỗi không xác định trong quá trình hoàn thành chuẩn bị đơn hàng, vui lòng liên hệ người quản trị website!";

                return Redirect("~/Admin/Order/OrderWaitPrepare");
            }
            catch (Exception e)
            {
                Log.Error("Have an error when confirm order in controller", e);
                return BadRequest();
            }
        }

        [Authorize(Roles = "Shipper")]
        [HttpGet]
        public IActionResult StartDeliveryOrder(int id)
        {
            try
            {
                var result = _orderService.StartDeliveryOrder(id);
                if (result == 1) ErrorMessage = "Đơn hàng đã chuyển sang trạng thái bắt đầu giao hàng.";
                if (result == 2) ErrorMessage = "Vui lòng chuẩn bị xong đơn hàng trước.";
                if (result == 3) ErrorMessage = "Đơn hàng đã được vận chuyển bởi một nhân viên khác.";
                if (result == -1) ErrorMessage = "Có lỗi không xác định trong quá trình hoàn thành chuẩn bị đơn hàng, vui lòng liên hệ người quản trị website!";

                return Redirect("~/Admin/Order/OrderDetail/?id=" + id + "&errorMessage=" + ErrorMessage);
            }
            catch (Exception e)
            {
                Log.Error("Have an error when confirm order in controller", e);
                return BadRequest();
            }
        }

        [Authorize(Roles = "Shipper")]
        [HttpGet]
        public IActionResult FinishDeliveryOrder(int id)
        {
            try
            {
                var result = _orderService.FinishDeliveryOrder(id);
                if (result == 1)
                {
                    return Redirect("~/Admin/Order/OrderWaitDelivery");
                }
                if (result == 2) ErrorMessage = "Vui lòng bắt đầu giao hàng trước.";
                if (result == 3) ErrorMessage = "Đơn hàng đã được hoàn thành bởi một nhân viên khác.";
                if (result == 4) ErrorMessage = "Chỉ có người nhận giao đơn hàng này mới có quyền hoàn thành việc chuẩn bị đơn.";
                if (result == -1) ErrorMessage = "Có lỗi không xác định trong quá trình hoàn thành chuẩn bị đơn hàng, vui lòng liên hệ người quản trị website!";

                return Redirect("~/Admin/Order/OrderDetail/?id=" + id + "&errorMessage=" + ErrorMessage);
            }
            catch (Exception e)
            {
                Log.Error("Have an error when confirm order in controller", e);
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult CancelOrder(int id, string content)
        {
            try
            {
                var result = _orderService.CancelOrder(id, content);
                if (result == 1) return Redirect("~/Admin/Order/OrderCancel");
                if (result == 2) ErrorMessage = "Đơn hàng đã hoàn thành không thể huỷ bỏ.";
                if (result == 3) ErrorMessage = "Đơn hàng đã được huỷ bởi một nhân viên khác.";
                if (result == 4) ErrorMessage = "Bạn không có quyền huỷ đơn hàng này.";
                if (result == -1) ErrorMessage = "Có lỗi không xác định trong quá trình huỷ đơn hàng, vui lòng liên hệ người quản trị website!";

                return Redirect("~/Admin/Order/OrderDetail/?id=" + id + "&errorMessage=" + ErrorMessage);
            }
            catch (Exception e)
            {
                Log.Error("Have an error when confirm order in controller", e);
                return BadRequest();
            }
        }
    }
}