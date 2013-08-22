using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;

namespace Glimpse.Issues
{
    public class PackageIssueProvider
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IIssueRepository _issueRepository;

        public PackageIssueProvider(IPackageRepository packageRepository, IIssueRepository issueRepository)
        {
            _packageRepository = packageRepository;
            _issueRepository = issueRepository;
        }

        public IEnumerable<GlimpsePackage> GetPackageIssues()
        {
            var packages = _packageRepository.GetAllPackages().ToList();
            var issues = _issueRepository.GetAllIssues();
            foreach (var issue in issues)
            {
                AddIssueToAssociatedPackage(packages, issue);
            }
            return packages;
        }


        private void AddIssueToAssociatedPackage(List<GlimpsePackage> packages, GithubIssue issue)
        {
            var labels = issue.Labels.Select(l => l.Name).ToList();

            foreach (var label in labels)
            {
                foreach (var package in packages)
                {
                    foreach (var tag in package.Tags)
                    {
                        if (tag.ToLower() == label.ToLower())
                        {
                            package.AddIssue(issue);
                        }
                    }
                }

            }
        }
    }
}