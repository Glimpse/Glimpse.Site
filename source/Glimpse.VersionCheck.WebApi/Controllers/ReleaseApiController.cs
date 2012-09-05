using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http; 
using Glimpse.VersionCheck.WebApi.Framework;

namespace Glimpse.VersionCheck.WebApi.Controllers
{
    [HttpHeader("Access-Control-Allow-Origin", "*")]
    public class ReleaseApiController : ApiController
    {
        [HttpGet]
        public LatestReleaseInfo Check([System.Web.Http.ModelBinding.ModelBinder(typeof(VersionCheckDetailsApiModelBinderProvider))] VersionCheckDetails details, bool withDetails) 
        {
            var service = GlimpseSettings.Settings.NewReleaseService;
            var result = service.GetLatestReleaseInfo(details, withDetails);
             
            var uri = Url.Request.RequestUri;
            result.ViewLink = String.Format("{0}://{1}/release/check/details{2}", uri.Scheme, uri.Authority, uri.Query);

            return result;
        } 
    }
}
