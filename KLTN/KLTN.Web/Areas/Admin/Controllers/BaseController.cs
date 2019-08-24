using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KLTN.Common;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Web.Areas.Admin.Controllers
{
    [Area(AreasNames.AreaAdmin)]
    [Route(RouteConfigs.AdminToController)]
    [Route(RouteConfigs.AdminToControllerToAction)]
    [Route(RouteConfigs.AdminToControllerToActionToId)]
    public class BaseController : Controller
    {
    }
}