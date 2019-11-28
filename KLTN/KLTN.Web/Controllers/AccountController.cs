using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml.Linq;
using KLTN.Common;
using KLTN.Common.Infrastructure;
using KLTN.DataModels.Models.Users;
using KLTN.Services;
using KLTN.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace KLTN.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        
        private readonly IHostingEnvironment _hostingEnvironment;


        
        public AccountController(IUserService userService, IHostingEnvironment hostingEnvironment)
        {
            _userService = userService;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET
        /// <summary>
        /// Action Authentication Get return View Authentication
        /// </summary>
        /// <param name="requestPath"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Authentication(string requestPath)
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Response.Cookies.Delete(CookieAuthenticationDefaults.AuthenticationScheme);
            //Transmission ViewBag to View
            try
            {
                var model = new AuthenticationViewModel();
                model.RedirectUrl = requestPath ?? Signs.Slash;
                return View(model);
            }
            catch (Exception e)
            {
                Log.Error(e, "Have an error when authentication");
                return BadRequest();
            }
        }

        /// <summary>
        /// Action Authentication Post Verify Authentication. If Authen success redirect to requestPath
        /// else return View with accountLoginModel
        /// </summary>
        /// <param name="accountLoginModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Authentication(AuthenticationViewModel authentication)
        {
            //Check ModelState if false return View with loginModel
            if (!ModelState.IsValid) return View(authentication);

            try
            {
                //Get account
                var account = _userService.Authentication(authentication);

                switch (account.Item2)
                {
                    case -1:
                        ModelState.AddModelError(Signs.DoubleQuotationMarks, "Tài khoản không tồn tại");
                        return View(authentication);
                    case -2:
                        ModelState.AddModelError(Signs.DoubleQuotationMarks, "Sai mật khẩu");
                        return View(authentication);
                    case 0:
                        ModelState.AddModelError(Signs.DoubleQuotationMarks, "Vui lòng kích hoạt email");
                        return View(authentication);
                    case -3:
                        ModelState.AddModelError(Signs.DoubleQuotationMarks, "Có lỗi không xác định, xin lỗi vì sự bất tiện này");
                        return View(authentication);
                    default:
                        var role = "";
                        switch (account.Item1.Role)
                        {
                            case 0:
                                role = "Admin";
                                break;
                            case 1:
                                role = "Manager";
                                break;
                            case 2:
                                role = "WareHouseStaff";
                                break;
                            case 3:
                                role = "Shipper";
                                break;
                            default:
                                role = "Customer";
                                break;

                        }
                        var claims = new List<Claim>
                        {
                            new Claim(Constants.Id, account.Item1.Id.ToString()),
                            new Claim(Constants.Email, account.Item1.Email),
                            new Claim(ClaimTypes.Role, role)
                        };

                        // create identity
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        // create principal
                        var principal = new ClaimsPrincipal(identity);

                        // sign-in
                        HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            principal,
                            new AuthenticationProperties());

                        return Redirect(authentication.RedirectUrl ?? Signs.Slash);
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Have an error when authentication");
                ModelState.AddModelError(Signs.DoubleQuotationMarks, "Có lỗi không xác định, hãy quay lại sau, xin lỗi vì sự bất tiện này");
                return View(authentication);
            }
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
        
        [HttpGet]
        public IActionResult ConfirmUser(string confirmString)
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult ConfirmUser()
        {
            var confirmString = HttpContext.Request.Path.ToString().Split("/")[3];
            var result = _userService.ConfirmUser(confirmString);
            if (result) return RedirectToAction("Index", "Home");
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(EmailModel emailModel)
        {
            if (!ModelState.IsValid) return View(emailModel);
            var result = _userService.ForgotPassword(emailModel.Email);
            if(result == 0)
            {
                ModelState.AddModelError("", "Email không tồn tại");
                return View(emailModel);
            }
            if(result==-1)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra trong quá trình lấy lại mật khẩu. Vui lòng quay lại sau!");
                return View(emailModel);
            }

            return RedirectToAction("ForgotOk","Notification");
        }


        public IActionResult RetypePassword(string confirmString)
        {
            var confirmStrings = HttpContext.Request.Path.ToString().Split("/")[3];
            var retypePassword = new RetypePassword
            {
                ConfirmString = confirmStrings
            };
            return View(retypePassword);
        }

        [HttpPost]
        public IActionResult RetypePassword(RetypePassword retypePassword)
        {
            if (!ModelState.IsValid) return View(retypePassword);
            var result = _userService.ConfirmForgotPassword(retypePassword);
            if (result) return RedirectToAction("RetypeSuccess","Notification");
            return View();
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
    }
}