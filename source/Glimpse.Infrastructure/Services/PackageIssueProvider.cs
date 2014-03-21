using System.Collections.Generic;
using System.Linq;
using Glimpse.Infrastructure.GitHub;
using Glimpse.Infrastructure.Repositories;

namespace Glimpse.Infrastructure
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

        public IEnumerable<GlimpsePackage> GetPackageIssues(int milestoneNumber)
        {
            var packages = _packageRepository.GetAllPackages().ToList();
            var issues = _issueRepository.GetAllIssuesFromMilestone(milestoneNumber);
            foreach (var issue in issues)
            {
                AddIssueToAssociatedPackage(packages, issue);
            }
            return packages;
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