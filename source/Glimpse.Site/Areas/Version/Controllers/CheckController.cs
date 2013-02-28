using System.Web.Mvc;
using Glimpse.Package;

namespace Glimpse.Site.Areas.Version.Controllers
{
    public partial class CheckController : Controller
    {
        public virtual ActionResult Index(VersionCheckDetails details, bool withDetails)
        {
            var service = PackageSettings.Settings.ReleaseQueryService;
            var result = service.GetReleaseInfo(details, true);

            // Indicates how much data we want to show
            ViewBag.WithDetails = withDetails;

            return View(MVC.Version.Check.Views.Index, MVC.Shared.Views._Simple, result);
        }

        public virtual ActionResult Release(string package, string version, string stamp)
        {
            var service = PackageSettings.Settings.ReleaseService;
            var result = service.GetReleaseInfo(package, version);

            return View(result);
        }
    }
}
