using System.Web.Mvc;
using Glimpse.Package;

namespace Glimpse.Site.Areas.Version.Controllers
{
    public partial class CheckController : Controller
    {
        public virtual ActionResult Index(VersionCheckDetails details, bool withDetails)
        {
            var service = PackageSettings.Settings.CheckingForReleaseService;
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
    }
}
