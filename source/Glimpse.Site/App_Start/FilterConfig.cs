using System.Web.Mvc;
using Glimpse.Site.Telemetry;

namespace Glimpse.Site
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ApplicationInsightsErrorAttribute());
        }
    }
}
