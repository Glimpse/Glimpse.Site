using System.Web.Mvc;

namespace Glimpse.Site.Areas.Version
{
    public class VersionAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Version";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Version_CheckWithDetails",
                "Version/Check/Details",
                new { controller = "Check", action = "Index", withDetails = true }
            ); 
            context.MapRoute(
                "Version_Check",
                "Version/Check",
                new { controller = "Check", action = "Index", withDetails = false }
            );

            context.MapRoute(
                "Version_default",
                "Version/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
