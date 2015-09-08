using System;
using System.Web.Mvc;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace Glimpse.Site.Telemetry
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class ApplicationInsightsErrorAttribute : HandleErrorAttribute
    {
        private TelemetryClient client = new TelemetryClient();

        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext != null && filterContext.HttpContext != null && filterContext.Exception != null)
            {
                //If customError is Off, then AI HTTPModule will report the exception
                if (filterContext.HttpContext.IsCustomErrorEnabled)
                {
                    ExceptionTelemetry exc = new ExceptionTelemetry(filterContext.Exception);
                    exc.HandledAt = ExceptionHandledAt.Platform;
                    client.TrackException(exc);
                }
                base.OnException(filterContext);
            }
        }
    }
}

