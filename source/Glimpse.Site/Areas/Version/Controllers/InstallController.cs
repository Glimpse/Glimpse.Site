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
        public virtual ActionResult Index(VersionCheckDetails details)
        {
            var service = PackageSettings.Settings.ReleaseQueryService;
            var result = service.GetReleaseInfo(details, false);

            return View(MVC.Version.Install.Views.Index, MVC.Shared.Views._Simple, result);
        }

        public virtual ActionResult Update(VersionCheckDetails details, bool? withDetails)
        {
            var service = PackageSettings.Settings.ReleaseQueryService;
            var result = service.GetReleaseInfo(details, true);

            // Indicates how much data we want to show
            ViewBag.WithDetails = withDetails;

            return View(MVC.Version.Install.Views.Update, MVC.Shared.Views._Simple, result);
        }
    }
}
