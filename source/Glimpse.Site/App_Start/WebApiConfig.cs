using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http; 

namespace Glimpse.Site
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "Version_ApiCheckWithDetails",
                "Api/Version/Check/Details/",
                new { controller = "CheckApi", action = "Index", withDetails = true }
            );

            config.Routes.MapHttpRoute(
                "Version_ApiCheck",
                "Api/Version/Check",
                new { controller = "CheckApi", action = "Index", withDetails = false }
            );

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );
        }
    }
}
