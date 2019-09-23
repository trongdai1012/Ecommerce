using KLTN.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Web.Areas.Admin.Controllers
{
    [Area(AreasNames.AreaAdmin)]
    [Route(RouteConfigs.AdminToController)]
    [Route(RouteConfigs.AdminToControllerToAction)]
    [Route(RouteConfigs.AdminToControllerToActionToId)]
    [Authorize]
    public class BaseController : Controller
    {
    }
}