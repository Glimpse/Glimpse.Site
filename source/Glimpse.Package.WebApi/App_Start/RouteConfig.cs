using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Glimpse.Package.WebApi
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "ReleaseCheckWithDetailsApi",
                routeTemplate: "api/release/check/details",
                defaults: new { controller = "ReleaseApi", action = "Check", withDetails = true }
            );

            routes.MapHttpRoute(
                name: "ReleaseCheckApi",
                routeTemplate: "api/release/check",
                defaults: new { controller = "ReleaseApi", action = "Check", withDetails = false }
            );

            routes.MapRoute(
                name: "ReleaseCheckWithDetails",
                url: "release/check/details",
                defaults: new { controller = "Release", action = "Check", withDetails = true }
            );

            routes.MapRoute(
                name: "ReleaseCheckTest",
                url: "release/check/test",
                defaults: new { controller = "Release", action = "Test" }
            );

            routes.MapRoute(
                name: "ReleaseCheckUpdate",
                url: "release/check/update",
                defaults: new { controller = "Release", action = "Update" }
            );

            routes.MapRoute(
                name: "ReleaseCheck",
                url: "release/check",
                defaults: new { controller = "Release", action = "Check", withDetails = false }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}