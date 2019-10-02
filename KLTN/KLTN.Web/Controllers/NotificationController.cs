using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Web.Controllers
{
    public class NotificationController : Controller
    {
        public IActionResult ForgotOk()
        {
            return View();
        }

        public IActionResult RetypeSuccess()
        {
            return View();
        }

        public IActionResult PaymentSuccess()
        {
            return View();
        }

        public IActionResult PaymentFailed()
        {
            return View();
        }
    }
}