using System;
using System.Collections.Generic;
using System.Web.Http;
using Glimpse.Package;
using Glimpse.Site.Framework;
using Microsoft.ApplicationInsights;

namespace Glimpse.Site.Areas.api.Controllers
{
    [HttpHeader("Access-Control-Allow-Origin", "*")]
    public class VersionController : ApiController
    {
        private TelemetryClient telemetry = new TelemetryClient();

        [HttpGet]
        public ReleaseQueryInfo Index([System.Web.Http.ModelBinding.ModelBinder(typeof(VersionCheckDetailsApiModelBinderProvider))] VersionCheckDetails details, bool withDetails = false)
        {
            string userId = null;

            if (HttpContext.Current.Request.Cookies["ai_user"] == null)
            {
                userId = Guid.NewGuid().ToString();
                var c = new HttpCookie("ai_user", userId + "|" + DateTime.Now.ToString("G"));
                c.Expires = DateTime.MaxValue;
                c.Path = "/";

                HttpContext.Current.Response.Cookies.Set(c);
            }

            foreach (var package in details.Packages)
            {
                EventTelemetry evt = new EventTelemetry();
                evt.Name = "Check " + package.Name;

                if (!string.IsNullOrEmpty(userId))
                {
                    evt.Context.User.Id = userId;
                }

                evt.Properties.Add("Package", package.Name);
                evt.Properties.Add ("Version", package.Version);

                if (HttpContext.Current.Request.UrlReferrer != null)
                {
                    evt.Properties.Add("urlReferrer", HttpContext.Current.Request.UrlReferrer.ToString());
                }
                telemetry.TrackEvent(evt);
            }
            var service = PackageSettings.Settings.ReleaseQueryService;
            var result = service.GetReleaseInfo(details, withDetails);

            result.ViewLink = GenerateViewUri(Url.Request.RequestUri, result);

            return result;
        }

        private string GenerateViewUri(Uri uri, ReleaseQueryInfo result)
        {
            var queryString = "";
            var spacer = "";
            foreach (var item in result.Details)
            {
                queryString += string.Format("{0}{1}={2}", spacer, item.Key, item.Value.Version);
                spacer = "&";
            }

            return String.Format("{0}://{1}/version/check?{2}", uri.Scheme, uri.Authority, queryString);
        }
    }
}
