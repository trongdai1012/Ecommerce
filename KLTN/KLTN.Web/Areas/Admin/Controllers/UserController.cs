using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KLTN.Common.Datatables;
using KLTN.DataAccess.Models;
using KLTN.DataModels.Models.Users;
using KLTN.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;

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

        public IActionResult AdminDetail(int id)
        {
            try
            {
                var admin = _userService.GetAdmin(id);
                switch (admin.Item2)
                {
                    case -1:
                        return BadRequest();
                    case 1:
                        return View(admin.Item1);
                    default:
                        return RedirectToAction("ListAdmin","User");
                }
            }
            catch (Exception e)
            {
                Log.Error("Have an error when get detail admin in controller", e);
                return BadRequest();
            }
        }

//        public IActionResult EmployeeDetail(int id)
//        {
//            return View();
//        }
//
//        public IActionResult CustomerDetail(int id)
//        {
//            return View();
//        }

        public IActionResult AdminUpdate(int id)
        {
            var admin = _userService.GetAdminUpdate(id);
            return View(admin.Item1);
        }

        public IActionResult AdminUpdate(UpdateAdminViewModel adminView)
        {
            if (!ModelState.IsValid) return View(adminView);
            try
            {
                var userResult = _userService.UpdateAdmin(adminView);
                switch(userResult)
                {
                    case 1:
                        return RedirectToAction("ListAdmin","User");
                    default:
                        return BadRequest();
                }

            }catch(Exception e)
            {
                Log.Error("Have an error when update admin in UserController", e);
                return BadRequest();
            }
        }
    }
}