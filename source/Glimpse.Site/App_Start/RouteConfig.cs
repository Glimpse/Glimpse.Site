using System.Web.Http;
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
                name: "Talk",
                url: "Talk",
                defaults: new { controller = "Home", action = "Talk" }
            );

            routes.MapRoute(
                name: "Protocol",
                url: "Protocol",
                defaults: new { controller = "Home", action = "Protocol" }
            );

            routes.MapRoute(
                name: "Documentation",
                url: "Help/{mdSlug}",
                defaults: new { controller = "Docs", action = "Index", mdSlug = "" }
            );

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}