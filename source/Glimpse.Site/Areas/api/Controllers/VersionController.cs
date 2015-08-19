using System;
using System.Web.Http;
using System.Collections.Generic;
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
            foreach (var package in details.Packages)
            {
                var properties = new Dictionary<string, string> { { "Package", package.Name }, { "Version", package.Version } };
                telemetry.TrackEvent("Check " + package.Name, properties);
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