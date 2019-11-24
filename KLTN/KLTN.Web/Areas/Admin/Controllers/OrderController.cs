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

        [HttpGet]
        public IActionResult ConfirmOrder(int id)
        {
            try
            {
                var result = _orderService.OrderConfirm(id);
                if (result == 1) ViewBag.ErrorMessage = "Xác thực đơn hàng thành công";
                if (result == -1) ViewBag.ErrorMessage = "Có lỗi không xác định trong quá trình xác thực đơn hàng, vui lòng liên hệ người quản trị website!";
                if (result == 0) ViewBag.ErrorMessage = "Đơn hàng đã được xác thực bởi một nhân viên khác.";

                return Redirect("~/Admin/Order/OrderDetail/"+id);
            }
            catch(Exception e)
            {
                Log.Error("Have an error when confirm order in controller", e);
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult StartPrepareOrder(int id)
        {
            try
            {
                var result = _orderService.StartPrepareOrder(id);
                if (result == 1) ViewBag.ErrorMessage = "Đã chuyển sang trạng thái chuẩn bị đơn hàng";
                if (result == 2) ViewBag.ErrorMessage = "Vui lòng xác thực đơn hàng trước.";
                if (result == 3) ViewBag.ErrorMessage = "Đơn hàng đã được chuẩn bị bởi một nhân viên khác";
                if (result == -1) ViewBag.ErrorMessage = "Có lỗi không xác định trong quá trình xác thực đơn hàng, vui lòng liên hệ người quản trị website!";

                return Redirect("~/Admin/Order/OrderDetail/" + id);
            }
            catch (Exception e)
            {
                Log.Error("Have an error when confirm order in controller", e);
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult FinishPrepareOrder(int id)
        {
            try
            {
                var result = _orderService.FinishPrepareOrder(id);
                if (result == 1) ViewBag.ErrorMessage = "Đã chuẩn bị xong đơn hàng.";
                if (result == 2) ViewBag.ErrorMessage = "Vui lòng chuẩn bị đơn hàng trước.";
                if (result == 3) ViewBag.ErrorMessage = "Đơn hàng đã được chuẩn bị xong bởi một nhân viên khác.";
                if (result == 4) ViewBag.ErrorMessage = "Chỉ có người chuẩn bị đơn hàng mới có quyền hoàn thành việc chuẩn bị đơn.";
                if (result == -1) ViewBag.ErrorMessage = "Có lỗi không xác định trong quá trình hoàn thành chuẩn bị đơn hàng, vui lòng liên hệ người quản trị website!";

                return Redirect("~/Admin/Order/OrderDetail/" + id);
            }
            catch (Exception e)
            {
                Log.Error("Have an error when confirm order in controller", e);
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult StartDeliveryOrder(int id)
        {
            try
            {
                var result = _orderService.StartDeliveryOrder(id);
                if (result == 1) ViewBag.ErrorMessage = "Đơn hàng đã chuyển sang trạng thái bắt đầu giao hàng.";
                if (result == 2) ViewBag.ErrorMessage = "Vui lòng chuẩn bị xong đơn hàng trước.";
                if (result == 3) ViewBag.ErrorMessage = "Đơn hàng đã được vận chuyển bởi một nhân viên khác.";
                if (result == -1) ViewBag.ErrorMessage = "Có lỗi không xác định trong quá trình hoàn thành chuẩn bị đơn hàng, vui lòng liên hệ người quản trị website!";

                return Redirect("~/Admin/Order/OrderDetail/" + id);
            }
            catch (Exception e)
            {
                Log.Error("Have an error when confirm order in controller", e);
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult FinishDeliveryOrder(int id)
        {
            try
            {
                var result = _orderService.FinishDeliveryOrder(id);
                if (result == 1) ViewBag.ErrorMessage = "Giao hàng thành công. Đơn hàng hoàn tất.";
                if (result == 2) ViewBag.ErrorMessage = "Vui lòng bắt đầu giao hàng trước.";
                if (result == 3) ViewBag.ErrorMessage = "Đơn hàng đã được hoàn thành bởi một nhân viên khác.";
                if (result == 4) ViewBag.ErrorMessage = "Chỉ có người nhận giao đơn hàng này mới có quyền hoàn thành việc chuẩn bị đơn.";
                if (result == -1) ViewBag.ErrorMessage = "Có lỗi không xác định trong quá trình hoàn thành chuẩn bị đơn hàng, vui lòng liên hệ người quản trị website!";

                return Redirect("~/Admin/Order/OrderDetail/" + id);
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
                if (result == 1) ViewBag.ErrorMessage = "Đơn hàng đã bị huỷ bỏ.";
                if (result == 2) ViewBag.ErrorMessage = "Đơn hàng đã hoàn thành không thể huỷ bỏ.";
                if (result == 3) ViewBag.ErrorMessage = "Đơn hàng đã được huỷ bởi một nhân viên khác.";
                if (result == 4) ViewBag.ErrorMessage = "Bạn không có quyền huỷ đơn hàng này.";
                if (result == -1) ViewBag.ErrorMessage = "Có lỗi không xác định trong quá trình huỷ đơn hàng, vui lòng liên hệ người quản trị website!";

                return Redirect("~/Admin/Order/OrderDetail/" + id);
            }
            catch (Exception e)
            {
                Log.Error("Have an error when confirm order in controller", e);
                return BadRequest();
            }
        }
    }
}