using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.Common
{
    public static class Constants
    {
        public const string DefaultConnection = "DefaultConnection";
    }

    public static class TypeOfSql
    {
        public const string VarChar = "VARCHAR";
        public const string NVarChar = "NVARCHAR";
        public const string Int = "INT";
        public const string Decimal = "DECIMAL";
        public const string Bool = "BOOL";
        public const string TinyInt = "TINYINT";
        public const string NText = "NTEXT";
    }

    public static class Paths
    {
        public const string PathLogs = "Logs/log-.txt";
    }

    public static class Settings
    {
        public const string NameSpaceWeb = "KLTN.Web";
    }

    public static class MapRoutesConfig
    {
        public const string HomeIndexId = "{controller=Home}/{action=Index}/{id?}";
        public const string AreasExistsHomeIndex = "{area:exists}/{controller=Home}/{action=Index}";
    }

    public static class MapRouteNames
    {
        public const string Default = "default";
        public const string AreaRoute = "areaRoute";
    }

    public static class RouteConfigs
    {
        public const string AdminToController = "admin/[controller]";
        public const string AdminToControllerToAction = "admin/[controller]/[action]";
        public const string AdminToControllerToActionToId = "admin/[controller]/[action]/{id}";
        public const string Error400 = "Error/400";
        public const string Error401 = "Error/401";
        public const string Error403 = "Error/403";
        public const string Error404 = "Error/404";
        public const string Error500 = "Error/500";
    }

    public static class AreasNames
    {
        public const string AreaAdmin = "Admin";
    }
}
