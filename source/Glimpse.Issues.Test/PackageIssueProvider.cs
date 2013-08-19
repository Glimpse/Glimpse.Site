using System.Collections.Generic;
using System.Linq;

namespace Glimpse.Issues.Test
{
    public class PackageIssueProvider
    {
        private readonly PackageRepository _packageRepository;
        private readonly IIssueRepository _cacheIssueRepository;

        public PackageIssueProvider(PackageRepository packageRepository, IIssueRepository cacheIssueRepository)
        {
            _packageRepository = packageRepository;
            _cacheIssueRepository = cacheIssueRepository;
        }

        public IEnumerable<GlimpsePackage> GetPackageIssues()
        {
            var issues = _cacheIssueRepository.GetAllIssues();
            var packages = _packageRepository.GetAllPackages().ToList();
            foreach (var issue in issues)
            {
                AddIssueToAssociatedPackage(packages, issue);
            }
            return packages;
        }

        private void AddIssueToAssociatedPackage(IEnumerable<GlimpsePackage> packages, GithubIssue issue)
        {
            var labels = issue.Labels.Select(l => l.Name).ToList();
            foreach (var package in packages)
            {
                foreach (var label in labels)
                {
                    if (package.Tag.Contains(label) || package.Category.Contains(label))
                    {
                        package.AddIssue(issue);
                    }
                }

            }
        }
    }
}