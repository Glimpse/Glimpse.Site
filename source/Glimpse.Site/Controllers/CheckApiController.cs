using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Glimpse.Package;
using Glimpse.Site.Framework;

namespace Glimpse.Site.Controllers
{
    [HttpHeader("Access-Control-Allow-Origin", "*")]
    public class CheckApiController : ApiController
    { 
        [HttpGet]
        public LatestReleaseInfo Index([System.Web.Http.ModelBinding.ModelBinder(typeof(VersionCheckDetailsApiModelBinderProvider))] VersionCheckDetails details, bool withDetails)
        {
            var service = PackageSettings.Settings.NewReleaseService;
            var result = service.GetLatestReleaseInfo(details, withDetails);

            result.ViewLink = GenerateViewUri(Url.Request.RequestUri, result);

            return result;
        } 

        private string GenerateViewUri(Uri uri, LatestReleaseInfo result)
        {
            var queryString = "";
            var spacer = "";
            foreach (var item in result.Details)
            {
                queryString += string.Format("{0}{1}={2}", spacer, item.Key, item.Value.Version);
                spacer = "&";
            }

            return  String.Format("{0}://{1}/release/check/details?{2}", uri.Scheme, uri.Authority, queryString);
        }
    }
}
