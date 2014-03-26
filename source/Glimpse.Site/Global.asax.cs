using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Glimpse.Blog;
using Glimpse.Build;
using Glimpse.Package;
using Glimpse.Release;
using Glimpse.Twitter;

namespace Glimpse.Site
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        { 
            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BindingConfig.RegisterGlobalBindings(ModelBinders.Binders, GlobalConfiguration.Configuration);
            FormatterConfig.RegisterFormatters(GlobalConfiguration.Configuration);

            ReleaseSettings.Settings.Options.PackageListingPath = Server.MapPath("~/Content/packages.json");
            ReleaseSettings.Settings.Initialize();

            PackageSettings.Settings.Initialize();

            TwitterSettings.Settings.Initialize();

            BuildSettings.Settings.Initialize();

            BlogSettings.Settings.Initialize();
        }
    }
}
