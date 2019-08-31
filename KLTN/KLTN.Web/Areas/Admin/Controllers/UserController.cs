using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KLTN.Common.Datatables;
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
        public IActionResult RegisterEmployee()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterEmployee(CreateEmployeeViewModel registerUser)
        {
            if (!ModelState.IsValid) return View(registerUser);
            _userService.CreateEmployeeAccount(registerUser);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult RegisterAdmin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterAdmin(CreateAdminViewModel registerUser)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Co loi gi do ko xac dinh");
                return View(registerUser);
            }
            _userService.CreateAdminAccount(registerUser);
            return RedirectToAction("Index", "Home");
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

        [HttpGet]
        public IActionResult ListAdmin()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ListEmployee()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ListCustomer()
        {
            return View();
        }
        /// <summary>
        /// Action LoadAdmin, get listAdmin to Json
        /// </summary>
        /// <param name="dtParameters"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult LoadAdmin([FromBody] DTParameters dtParameters)
        {
            var tupleData = _userService.LoadAdmin(dtParameters);

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

        /// <summary>
        /// Action LoadAdmin, get listAdmin to Json
        /// </summary>
        /// <param name="dtParameters"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult LoadCustomer([FromBody] DTParameters dtParameters)
        {
            var tupleData = _userService.LoadCustomer(dtParameters);

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

        /// <summary>
        /// Action LoadAdmin, get listAdmin to Json
        /// </summary>
        /// <param name="dtParameters"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult LoadEmployee([FromBody] DTParameters dtParameters)
        {
            var tupleData = _userService.LoadEmployee(dtParameters);

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

        /// <summary>
        /// Action ChangeStatus return JsonResult to ajax
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var statusResult = _userService.ChangeStatus(id);

            return Json(new
            {
                status = statusResult
            });
        }


    }
}