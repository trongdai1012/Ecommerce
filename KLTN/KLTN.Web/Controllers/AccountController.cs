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
                ViewBag.RequestPath = requestPath ?? Signs.Slash;
                return View();
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
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, account.Item1.Id.ToString()),
                            new Claim(Constants.Id, account.Item1.Id.ToString()),
                            new Claim(Constants.Email, account.Item1.Email),
                            new Claim(Constants.Role, account.Item1.Role.ToString())
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
            return RedirectToAction("Index","Home");
        }
        
        public JsonResult LoadProvince()
        {
            var xmlDoc = XDocument.Load(Path.Combine(_hostingEnvironment.WebRootPath,"Provinces_Data.xml"));

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
        public JsonResult LoadDistrict(int provinceID)
        {
            var xmlDoc = XDocument.Load(Path.Combine(_hostingEnvironment.WebRootPath,"Provinces_Data.xml"));

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
    }
}