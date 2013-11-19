using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;

namespace Glimpse.Issues
{
    public class PackageIssueProvider
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IIssueRepository _issueRepository;
        private readonly IGithubMilestoneService _githubMilestoneService;

        public PackageIssueProvider(IPackageRepository packageRepository, IIssueRepository issueRepository, IGithubMilestoneService githubMilestoneService)
        {
            _packageRepository = packageRepository;
            _issueRepository = issueRepository;
            _githubMilestoneService = githubMilestoneService;
        }

        public IEnumerable<GlimpsePackage> GetLatestPackageIssues()
        {
            var packages = _packageRepository.GetAllPackages().ToList();
            var latestMilestoneWithIssues = GetLatestMilestoneWithIssues();
            var issues = _issueRepository.GetAllIssuesFromMilestone(latestMilestoneWithIssues);
            foreach (var issue in issues)
            {
                AddIssueToAssociatedPackage(packages, issue);
            }
            return packages;
        }

        private int GetLatestMilestoneWithIssues()
        {
            return (from g in _githubMilestoneService.GetMilestones()
                   where g.Open_Issues > 0 || g.Closed_Issues > 0
                   orderby g.Created_At descending 
                   select g.Number).First();
        }

        private void AddIssueToAssociatedPackage(IEnumerable<GlimpsePackage> packages, GithubIssue issue)
        {
            var labels = issue.Labels.Select(l => l.Name).ToList();

            var glimpsePackageIssues = from label in labels 
                                       from package in packages 
                                       from tag in package.Tags
                                       where tag.ToLower() == label.ToLower() 
                                       select package;
            foreach (var package in glimpsePackageIssues)
            {
                package.AddIssue(issue);
            }
        }
    }
}