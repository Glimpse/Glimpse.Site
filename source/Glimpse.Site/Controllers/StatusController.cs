using System.Linq;
using System.Web.Mvc;
using Glimpse.Issues;
using Glimpse.Site.Framework;

namespace Glimpse.Site.Controllers
{
    public class StatusController : Controller
    {
        private readonly GlimpsePackageViewModelMapper _glimpsePackageViewModelMapper = new GlimpsePackageViewModelMapper();

        [OutputCache(Duration = 30 * 60)]
        public ActionResult Index()
        {
            var packageIssueProvider = CreatePackageIssueProvider();
            var glimpsePackages = packageIssueProvider.GetPackageIssues();
            var statusView = _glimpsePackageViewModelMapper.ConvertToIndexViewModel(glimpsePackages.ToList());
            return View(statusView);
        }

        public ActionResult InvalidateCacheForIndex()
        {
            Response.RemoveOutputCacheItem(Url.Action("index"));
            return RedirectToAction("Index");
        }

        private PackageIssueProvider CreatePackageIssueProvider()
        {
            var jsonFile = Server.MapPath("~/Content/packages.json");
            var basicHttpClient = new BasicHttpClient("https://api.github.com/", "application/json");
            return new PackageIssueProvider(new PackageRepository(jsonFile), new IssueRepository(new GithubIssueService(basicHttpClient), new GithubMilestoneService(basicHttpClient)));
        }
    }
}
