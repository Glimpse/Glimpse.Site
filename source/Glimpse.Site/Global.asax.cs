using System;
using System.Threading;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Glimpse.Blog;
using Glimpse.Build;
using Glimpse.Contributor;
using Glimpse.Package;
using Glimpse.Release;
using Glimpse.Twitter;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Glimpse.Site
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        { 
            // Read instrumentation key from azure web app settings
            string ikeyValue = Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY");

            if (!string.IsNullOrEmpty(ikeyValue))
            {
                TelemetryConfiguration.Active.InstrumentationKey = ikeyValue;
            }

            try
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

                PackageSettings.Settings.NugetListingPath = Server.MapPath("~/Content/nuget.json");
                PackageSettings.Settings.Initialize();

                TwitterSettings.Settings.Initialize();

                BuildSettings.Settings.Initialize();

                BlogSettings.Settings.Initialize();

                ContributorSettings.Settings.Options.ContributorListingPath = Server.MapPath("~/Content/committers.json");
                ContributorSettings.Settings.Initialize();
            }
            catch (Exception ex)
            {
                TelemetryClient client = new TelemetryClient();
                ExceptionTelemetry excTelemetry = new ExceptionTelemetry(ex);
                excTelemetry.HandledAt = ExceptionHandledAt.Unhandled;
                client.TrackException(excTelemetry);

                // this exception will terminate the process. Let's wait till telemetry will be delivered
                client.Flush();
                Thread.Sleep(1000);

                throw;
            }
        }
    }
}
