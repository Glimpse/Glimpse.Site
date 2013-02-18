using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Glimpse.Package;

namespace Glimpse.Site.Areas.Version.Controllers
{
    public partial class InstallController : Controller
    { 
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Update(VersionCheckDetails details, bool? withDetails)
        {
            var service = PackageSettings.Settings.NewReleaseService;
            var result = service.GetLatestReleaseInfo(details, withDetails ?? false);

            // Indicates how much data we want to show
            ViewBag.WithDetails = withDetails;

            return View(result);
        }
    }
}
