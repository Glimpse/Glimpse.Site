using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Glimpse.VersionCheck.WebApi.Framework;

namespace Glimpse.VersionCheck.WebApi.Controllers
{
    public class ReleaseApiController : ApiController
    {
        [HttpGet]
        public LatestReleaseInfo Check([ModelBinder(typeof(VersionCheckDetailsApiModelBinderProvider))] VersionCheckDetails details, bool withDetails) 
        {
            var service = GlimpseSettings.Settings.NewReleaseService;
            var result = service.GetLatestReleaseInfo(details, withDetails);

            return result;
        } 
    }
}
