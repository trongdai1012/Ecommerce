using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using KLTN.Common.Datatables;
using KLTN.Common.Infrastructure;
using KLTN.DataAccess.Models;
using KLTN.DataModels.Models.Users;
using KLTN.Services;
using KLTN.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;

namespace KLTN.Web.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        private readonly IHostingEnvironment _hostingEnvironment;

        public UserController(IUserService userService, IHostingEnvironment hostingEnvironment)
        {
            _userService = userService;
            _hostingEnvironment = hostingEnvironment;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid) return View();
            _userService.Register(registerUser);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ConfirmUser(string confirmString)
        {
            return View();
        }

        [AllowAnonymous]
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
                        return RedirectToAction("ListAdmin", "User");
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
                switch (userResult)
                {
                    case 1:
                        return RedirectToAction("ListAdmin", "User");
                    default:
                        return BadRequest();
                }

            }
            catch (Exception e)
            {
                Log.Error("Have an error when update admin in UserController", e);
                return BadRequest();
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public JsonResult LoadProvince()
        {
            var xmlDoc = XDocument.Load(Path.Combine(_hostingEnvironment.WebRootPath, "Provinces_Data.xml"));

            var xElements = xmlDoc.Element("Root").Elements("Item").Where(x => x.Attribute("type").Value == "province");
            var list = xElements.Select(item => new ProvinceModel
            {
                ID = int.Parse(item.Attribute("id").Value),
                Name = item.Attribute("value").Value
            })
                .ToList();
            return Json(new
            {
                data = list,
                status = true
            });
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult LoadDistrict(int provinceID)
        {
            var xmlDoc = XDocument.Load(Path.Combine(_hostingEnvironment.WebRootPath, "Provinces_Data.xml"));

            var xElement = xmlDoc.Element("Root").Elements("Item")
                .Single(x => x.Attribute("type").Value == "province" && int.Parse(x.Attribute("id").Value) == provinceID);

            var list = xElement.Elements("Item")
                .Where(x => x.Attribute("type").Value == "district")
                .Select(item => new DistrictModel
                {
                    ID = int.Parse(item.Attribute("id").Value),
                    Name = item.Attribute("value").Value,
                    ProvinceID = int.Parse(xElement.Attribute("id").Value)
                })
                .ToList();
            return Json(new
            {
                data = list,
                status = true
            });
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult LoadPrecinct(int districtID)
        {
            var xmlDoc = XDocument.Load(Path.Combine(_hostingEnvironment.WebRootPath, "Provinces_Data.xml"));

            var xElement = xmlDoc.Element("Root").Elements("Item").Elements("Item")
                .Single(x => x.Attribute("type").Value == "district" && int.Parse(x.Attribute("id").Value) == districtID);

            var list = xElement.Elements("Item")
                .Where(x => x.Attribute("type").Value == "precinct")
                .Select(item => new PrecinctModel
                {
                    Id = int.Parse(item.Attribute("id").Value),
                    Name = item.Attribute("value").Value,
                    DistrictId = int.Parse(xElement.Attribute("id").Value)
                })
                .ToList();
            return Json(new
            {
                data = list,
                status = true
            });
        }

        [HttpGet]
        public IActionResult UpdateRole(int id)
        {
            var user = _userService.GetUserUpdateRole(id);
            var roles = from EnumRole d in Enum.GetValues(typeof(EnumRole)) select new { Id = (int)d, Name = d.ToString() };

            ViewBag.Roles = new SelectList(roles, "Id", "Name", user.Role);

            return View(user);
        }

        [HttpPost]
        public IActionResult UpdateRole(User userModel)
        {
            try
            {
                var result = _userService.UpdateRole(userModel.Id, userModel.Role);
                if (result == null)
                {
                    var user = _userService.GetUserUpdateRole(userModel.Id);

                    var roles = from EnumRole d in Enum.GetValues(typeof(EnumRole)) select new { Id = (int)d, Name = d.ToString() };

                    ViewBag.Roles = new SelectList(roles, "Id", "Name", user.Role);

                    ModelState.AddModelError("", "Cập nhật thất bại.");
                    return View(userModel);
                }

                if (result.Role == 0)
                {
                    return RedirectToAction("ListAdmin", "User");
                }

                if (result.Role == 5)
                {
                    return RedirectToAction("ListCustomer", "User");
                }

                return RedirectToAction("ListEmployee", "User");
            }
            catch (Exception e)
            {
                Log.Error("Have an error when update role in controller", e);
                return BadRequest();
            }
        }
    }
}