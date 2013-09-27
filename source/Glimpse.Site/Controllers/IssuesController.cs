using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Glimpse.Issues;
using Glimpse.Site.Framework;

namespace Glimpse.Site.Controllers
{
    public class IssuesController : Controller
    {
        private readonly GlimpsePackageViewModelMapper _glimpsePackageViewModelMapper = new GlimpsePackageViewModelMapper();

        [OutputCache(Duration = 30 * 60)]
        public ActionResult Index()
        {
            var jsonFile = Server.MapPath("~/Content/packages.json");
            var packageIssueProvider = new PackageIssueProvider(new PackageRepository(jsonFile), new IssueRepository(new GithubIssueService()));
            var glimpsePackages = packageIssueProvider.GetPackageIssues();
            var glimpsePackageList = glimpsePackages.ToList();
            var issuesView = _glimpsePackageViewModelMapper.ConvertToIndexViewModel(glimpsePackageList);
            var users = new Dictionary<string, GithubUser>();
            foreach (var package in glimpsePackageList)
            {
                var packageReports = package.Issues.Select(i => i.User);
                foreach (var packageReport in packageReports)
                {
                    if (!users.ContainsKey(packageReport.Id))
                    {
                        users.Add(packageReport.Id, packageReport);
                    }
                }
            }
            issuesView.IssueReporters = users.Values.ToList();
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
