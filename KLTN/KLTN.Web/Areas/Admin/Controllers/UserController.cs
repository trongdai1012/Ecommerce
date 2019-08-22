using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KLTN.DataAccess.Models;
using KLTN.DataModels.Models.Users;
using KLTN.Services;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Web.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid) return View();
            _userService.Register(registerUser);
            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public IActionResult ConfirmUser(string confirmString)
        {
            return View();
        }

        [HttpPost]
        public IActionResult ConfirmUser()
        {
            var confirmString = HttpContext.Request.Path.ToString().Split("/")[4];
            var result = _userService.ConfirmUser(confirmString);
            if (result) return RedirectToAction("Index", "Home");
            return View();
        }
    }
}