using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Glimpse.Site
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
             
            routes.MapRoute(
                name: "Documentation",
                url: "Docs/",
                defaults: new { controller = "Docs", action = "Index" }
            );

            routes.MapRoute(
                name: "DocumentationDetails",
                url: "Docs/{mdSlug}",
                defaults: new { controller = "Docs", action = "Details", mdSlug = "" }
            );

            routes.MapRoute(
                name: "GettingStarted",
                url: "getting-started",
                defaults: new { controller = "GettingStarted", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
