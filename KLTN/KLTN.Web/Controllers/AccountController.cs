﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Authentication()
        {
            return View();
        }
    }
}