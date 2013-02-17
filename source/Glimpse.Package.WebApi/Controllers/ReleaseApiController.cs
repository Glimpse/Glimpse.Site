using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http; 
using Glimpse.Package.WebApi.Framework;

namespace Glimpse.Package.WebApi.Controllers
{
    [HttpHeader("Access-Control-Allow-Origin", "*")]
    public class ReleaseApiController : ApiController
    {
        [HttpGet]
        public LatestReleaseInfo Check([System.Web.Http.ModelBinding.ModelBinder(typeof(VersionCheckDetailsApiModelBinderProvider))] VersionCheckDetails details, bool withDetails) 
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
