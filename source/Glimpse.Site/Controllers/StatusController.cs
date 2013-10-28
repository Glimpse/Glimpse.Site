using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Glimpse.Issues;
using Glimpse.Site.Framework;
using Glimpse.Site.Models;

namespace Glimpse.Site.Controllers
{
    public class StatusController : Controller
    {
        private readonly GlimpsePackageViewModelMapper _glimpsePackageViewModelMapper = new GlimpsePackageViewModelMapper();

        [OutputCache(Duration = 30 * 60)]
        public ActionResult Index()
        {
            var jsonFile = Server.MapPath("~/Content/packages.json");
            var packageIssueProvider = new PackageIssueProvider(new PackageRepository(jsonFile), new IssueRepository(new GithubIssueService(), new GithubMilestoneService()));
            var glimpsePackages = packageIssueProvider.GetPackageIssues();
            var statusView = _glimpsePackageViewModelMapper.ConvertToIndexViewModel(glimpsePackages.ToList());
            return View(statusView);
        }

        public ActionResult InvalidateCacheForIndex()
        {
            Response.RemoveOutputCacheItem(Url.Action("index"));
            return RedirectToAction("Index");
        }
    }
}
