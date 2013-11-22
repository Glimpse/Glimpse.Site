using System.Collections.Generic;
using System.Configuration;
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
        private GithubMilestoneService _githubMilestoneService;

        [OutputCache(Duration = 30 * 60)]
        public ActionResult Index()
        {
            var statusView = SetupStatusDashboard();
            return View(statusView);
        }

        public ActionResult InvalidateCacheForIndex()
        {
            Response.RemoveOutputCacheItem(Url.Action("index"));
            return RedirectToAction("Index");
        }

        private IssuesIndexViewModel SetupStatusDashboard()
        {
            var packageIssueProvider = CreatePackageIssueProvider();
            var milestone = _githubMilestoneService.GetMilestone("vnext");
            var glimpsePackages = packageIssueProvider.GetPackageIssues(milestone.Number).ToList();
            var numberOfIssues = glimpsePackages.Sum(g => g.Issues.Count);
            if (numberOfIssues == 0)
            {
                milestone = _githubMilestoneService.GetLatestMilestoneWithIssues("closed");
                glimpsePackages = packageIssueProvider.GetPackageIssues(milestone.Number).ToList();
            }
            var statusView = _glimpsePackageViewModelMapper.ConvertToIndexViewModel(glimpsePackages.ToList());
            statusView.CurrentMilestone = milestone;
            return statusView;
        }

        private PackageIssueProvider CreatePackageIssueProvider()
        {
            var jsonFile = Server.MapPath("~/Content/packages.json");
            string githubKey = ConfigurationManager.AppSettings.Get("GithubKey");
            string githubSecret = ConfigurationManager.AppSettings.Get("GithubSecret");
            var httpClient = new HttpClientFactory().CreateHttpClient(githubKey, githubSecret);
            _githubMilestoneService = new GithubMilestoneService(httpClient);
            return new PackageIssueProvider(new PackageRepository(jsonFile), new IssueRepository(new GithubIssueService(httpClient), _githubMilestoneService), _githubMilestoneService);
        }
    }
}
