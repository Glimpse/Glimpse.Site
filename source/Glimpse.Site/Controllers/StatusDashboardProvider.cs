using System.Configuration;
using System.Linq;
using Glimpse.Infrastructure;
using Glimpse.Infrastructure.GitHub;
using Glimpse.Infrastructure.Repositories;
using Glimpse.Site.Framework;
using Glimpse.Site.Models;

namespace Glimpse.Site.Controllers
{
    public class StatusDashboardProvider
    {
        private readonly GlimpsePackageViewModelMapper _glimpsePackageViewModelMapper = new GlimpsePackageViewModelMapper();
        private GithubMilestoneService _githubMilestoneService;

        public IssuesIndexViewModel SetupStatusDashboard(string packagesFile)
        {
            var packageIssueProvider = CreatePackageIssueProvider(packagesFile);
            var milestone = _githubMilestoneService.GetMilestone("vnext");
            var glimpsePackages = Enumerable.ToList<GlimpsePackage>(packageIssueProvider.GetPackageIssues(milestone.Number));
            var numberOfIssues = glimpsePackages.Sum(g => g.Issues.Count);
            if (numberOfIssues == 0)
            {
                milestone = _githubMilestoneService.GetLatestMilestoneWithIssues("closed");
                glimpsePackages = Enumerable.ToList<GlimpsePackage>(packageIssueProvider.GetPackageIssues(milestone.Number));
            }
            var statusView = _glimpsePackageViewModelMapper.ConvertToIndexViewModel(glimpsePackages.ToList());
            statusView.CurrentMilestone = milestone;
            return statusView;
        }

        private PackageIssueProvider CreatePackageIssueProvider(string packagesFile)
        {
            string githubKey = ConfigurationManager.AppSettings.Get("GithubKey");
            string githubSecret = ConfigurationManager.AppSettings.Get("GithubSecret");
            var httpClient = new HttpClientFactory().CreateHttpClient(githubKey, githubSecret);
            _githubMilestoneService = new GithubMilestoneService(httpClient);
            return new PackageIssueProvider(new PackageRepository(packagesFile), new IssueRepository(new GithubIssueService(httpClient), _githubMilestoneService), _githubMilestoneService);
        }
    }
}