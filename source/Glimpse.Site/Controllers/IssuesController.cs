using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Glimpse.Issues;
using Glimpse.Site.Framework;
using Glimpse.Site.Models;

namespace Glimpse.Site.Controllers
{
    public class IssuesController : Controller
    {
        private readonly GlimpsePackageViewModelMapper _glimpsePackageViewModelMapper = new GlimpsePackageViewModelMapper();

//        [OutputCache(Duration = 30 * 60)]
        public ActionResult Index()
        {
            var jsonFile = Server.MapPath("~/Content/packages.json");
            var packageIssueProvider = new PackageIssueProvider(new PackageRepository(jsonFile), new IssueRepository(new GithubIssueService(), new GithubMilestoneService()));
            var glimpsePackages = packageIssueProvider.GetPackageIssues();
            var glimpsePackageList = glimpsePackages.ToList();
            var issuesView = _glimpsePackageViewModelMapper.ConvertToIndexViewModel(glimpsePackageList);
            return View(issuesView);
        }

        public ActionResult InvalidateCacheForIndex()
        {
            string path = Url.Action("index");
            Response.RemoveOutputCacheItem(path);
            return RedirectToAction("Index");
        }
    }
}
