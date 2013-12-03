using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Infrastructure;
using Glimpse.Infrastructure.GitHub;
using Glimpse.Site.Models;

namespace Glimpse.Site.Framework
{
    public class GlimpsePackageViewModelMapper
    {
        private IEnumerable<GlimpsePackage> _glimpsePackages;
        private List<string> _packageTags;

        public IssuesIndexViewModel ConvertToIndexViewModel(IEnumerable<GlimpsePackage> glimpsePackages)
        {
            var issuesView = new IssuesIndexViewModel();
            SetupFields(glimpsePackages);
            foreach (var glimpsePackage in _glimpsePackages)
            {
                var packageCategory = GetOrCreatePackageCategoryViewModel(issuesView, glimpsePackage);
                AddIssuesToViewModel(glimpsePackage, packageCategory);
                var packageView = new PackageViewModel
                {
                    Name = glimpsePackage.Title,
                    Status = glimpsePackage.Status,
                    StatusDescription = glimpsePackage.StatusDescription
                };
                packageCategory.Packages.Add(packageView);
            }
            issuesView.IssueReporters = GetIssueReporters(_glimpsePackages);
            var pullRequestContributors = new Dictionary<string, Tuple<GithubUser,List<GithubIssue>>>();
            foreach (var package in _glimpsePackages)
            {
                var issues = package.Issues.Where(i => i.Html_Url.Contains("/pull/"));
                foreach (var issue in issues)
                {
                    if (!pullRequestContributors.ContainsKey(issue.User.Id))
                        pullRequestContributors.Add(issue.User.Id, new Tuple<GithubUser, List<GithubIssue>>(issue.User, new List<GithubIssue>()));
                    if(pullRequestContributors[issue.User.Id].Item2.All(i => i.Id != issue.Id))
                     pullRequestContributors[issue.User.Id].Item2.Add(issue);
                }
            }
            issuesView.PullRequestContributors = pullRequestContributors.Values.ToList();
            return issuesView;
        }

        private List<GithubUser> GetIssueReporters(IEnumerable<GlimpsePackage> glimpsePackageList)
        {
            var users = new Dictionary<string, GithubUser>();
            foreach (var package in glimpsePackageList)
            {
                var packageReports = package.Issues.Where(i => i.Html_Url.Contains("/issues/")).Select(i => i.User);
                foreach (var packageReport in packageReports)
                {
                    if (!users.ContainsKey(packageReport.Id))
                    {
                        users.Add(packageReport.Id, packageReport);
                    }
                }
            }
            return users.Values.ToList();
        }

        private void SetupFields(IEnumerable<GlimpsePackage> glimpsePackages)
        {
            _glimpsePackages = glimpsePackages.ToList();
            _packageTags = new List<string>();
            foreach (var tags in _glimpsePackages.Select(g => g.Tags))
                _packageTags.AddRange(tags);
        }

        private void AddIssuesToViewModel(GlimpsePackage packageIssue, PackageCategoryViewModel packageCategory)
        {
            var openIssues = packageIssue.Issues.Where(p => p.Status == GithubIssueStatus.Open).ToList();
            var closedIssues = packageIssue.Issues.Where(p => p.Status == GithubIssueStatus.Closed).ToList();
            AddToIssueViewModel(openIssues, packageCategory.AcknowledgedIssues);
            AddToIssueViewModel(closedIssues, packageCategory.CompletedIssues);
        }

        private PackageCategoryViewModel GetOrCreatePackageCategoryViewModel(IssuesIndexViewModel issuesView, GlimpsePackage packageIssue)
        {
            var packageCategory = issuesView.PackageCategories.FirstOrDefault(c => c.Name == packageIssue.Category);
            if (packageCategory == null)
            {
                packageCategory = new PackageCategoryViewModel { Name = packageIssue.Category };
                issuesView.PackageCategories.Add(packageCategory);
            }
            return packageCategory;
        }

        private void AddToIssueViewModel(List<GithubIssue> openIssues, List<IssueViewModel> issueViewModels)
        {
            foreach (var openIssue in openIssues)
            {
                var issueView = ConvertToIssueViewModel(openIssue);
                if (issueViewModels.Count(p => p.IssueId == openIssue.Id) == 0)
                    issueViewModels.Add(issueView);
            }
        }

        private IssueViewModel ConvertToIssueViewModel(GithubIssue openIssue)
        {
            var issueView = new IssueViewModel();
            issueView.IssueId = openIssue.Id;
            issueView.Number = openIssue.Number;
            issueView.IssueLinkUrl = openIssue.Html_Url;
            foreach (var label in openIssue.Labels)
            {
                if (!_packageTags.Contains(label.Name))
                    issueView.Category += label.Name + ",";
            }
            RemoveLastComma(issueView);
            issueView.Description = openIssue.Title;
            issueView.User = openIssue.User;
            return issueView;
        }

        private static void RemoveLastComma(IssueViewModel issueView)
        {
            if (!string.IsNullOrEmpty(issueView.Category))
            {
                var lastComma = issueView.Category.Length - 1;
                issueView.Category = issueView.Category.Substring(0, lastComma);
            }
        }
    }
}