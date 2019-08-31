using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KLTN.DataModels.Models.Users;
using KLTN.Services;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Authentication()
        {
            return View();
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
            return RedirectToAction("Index", "Home");
        }
    }
}