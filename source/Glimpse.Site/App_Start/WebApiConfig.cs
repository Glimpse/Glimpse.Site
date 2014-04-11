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
                "api/version/check/details/",
                new { controller = "Version", action = "Index", withDetails = true }
            );
             
            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );
        }
    }
}
