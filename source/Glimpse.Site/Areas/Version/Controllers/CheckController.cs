using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Glimpse.Package;
using Glimpse.Site.Framework;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Glimpse.Site.Areas.Version.Controllers
{
    public partial class CheckController : Controller
    {
        public virtual ActionResult Index(VersionCheckDetails details, bool withDetails)
        {
            var service = PackageSettings.Settings.NewReleaseService;
            var result = service.GetLatestReleaseInfo(details, withDetails);

            // Indicates how much data we want to show
            ViewBag.WithDetails = withDetails;

            return View(result);
        }

        public virtual ActionResult Release(string package, string version, string stamp)
        {
            var service = PackageSettings.Settings.ReleaseService;
            var result = service.GetReleaseInfo(package, version);

            return View(result);
        }

        //public ActionResult Query(VersionCheckDetails details, bool withDetails)
        //{
        //    var service = PackageSettings.Settings.NewReleaseService;
        //    var result = service.GetLatestReleaseInfo(details, withDetails);

        //    result.ViewLink = GenerateViewUri(ControllerContext.HttpContext.Request.Url, result);

        //    return new JsonpResult(result);
        //}

        //private string GenerateViewUri(Uri uri, LatestReleaseInfo result)
        //{
        //    var queryString = "";
        //    var spacer = "";
        //    foreach (var item in result.Details)
        //    {
        //        queryString += string.Format("{0}{1}={2}", spacer, item.Key, item.Value.Version);
        //        spacer = "&";
        //    }

        //    return String.Format("{0}://{1}/release/check/details?{2}", uri.Scheme, uri.Authority, queryString);
        //}
    }
}
