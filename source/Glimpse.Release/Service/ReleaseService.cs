using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace Glimpse.Release
{
    public class ReleaseService : IReleaseService
    {
        private readonly IMilestoneProvider _milestoneProvider;
        private readonly IIssueProvider _issueProvider;
        private readonly IPackageProvider _packageProvider;
        private readonly object _releasesLock = new object();
        private readonly IDictionary<string, Release> _releases = new Dictionary<string, Release>(); 
        private const string NextMilestone = "vNext";

        public ReleaseService(IMilestoneProvider milestoneProvider, IIssueProvider issueProvider, IPackageProvider packageProvider)
        {
            _milestoneProvider = milestoneProvider;
            _issueProvider = issueProvider;
            _packageProvider = packageProvider;
        }

        public Release GetRelease(string milestoneNumber)
        {
            if (string.IsNullOrEmpty(milestoneNumber) || milestoneNumber == NextMilestone)
                milestoneNumber = NextMilestone;

            var release = (Release)null;
            if (!_releases.TryGetValue(milestoneNumber, out release))
            {
                lock (_releasesLock)
                {
                    if (!_releases.TryGetValue(milestoneNumber, out release))
                    {
                        release = BuildRelease(milestoneNumber);
                        _releases.Add(milestoneNumber, release);
                    }
                }
            }

            return release;
        }

        public void Clear()
        {
            _releases.Clear();
            _milestoneProvider.Clear();
            _issueProvider.Clear();
        }
         
        private Release BuildRelease(string milestoneNumber)
        {
            var milestone = (GithubMilestone)null;
            var issues = (IList<GithubIssue>)null;

            // Fetch the data that we need
            if (string.IsNullOrEmpty(milestoneNumber) || milestoneNumber == NextMilestone)
            {
                milestone = _milestoneProvider.GetMilestone(NextMilestone);
                issues = milestone != null ? _issueProvider.GetAllIssuesByMilestoneThatHasTag(milestone.Number, _packageProvider.GetAllPackagesTags()) : new List<GithubIssue>();
                if (!issues.Any())
                {
                    milestone = _milestoneProvider.GetLatestMilestoneWithIssues("closed");
                    issues = milestone != null ? _issueProvider.GetAllIssuesByMilestoneThatHasTag(milestone.Number, _packageProvider.GetAllPackagesTags()) : new List<GithubIssue>();
                }
            }
            else
            {
                milestone = _milestoneProvider.GetMilestone(milestoneNumber);
                issues = milestone != null ? _issueProvider.GetAllIssuesByMilestoneThatHasTag(milestone.Number, _packageProvider.GetAllPackagesTags()) : new List<GithubIssue>();
            }

            var packageCategories = _packageProvider.GetAllPackagesGroupedByCategory();
            var packageTags = _packageProvider.GetAllPackagesTags();

            // Lets map it to the format we need
            var release = new Release
            {
                Milestone = MapMilestone(milestone),
                IssueReporters = MapIssueReporters(issues),
                PullRequestContributors = MapPullRequestContributors(issues, packageTags),
                PackageCategories = MapCategories(packageCategories, packageTags, issues)
            };

            return release;
        }

        private Tuple<GithubMilestone, IList<GithubIssue>> GetMilestoneAndIssues(string milestoneNumber)
        {
            var milestone = _milestoneProvider.GetMilestone(milestoneNumber);

            var issues = (IList<GithubIssue>)null;
            if (milestone != null)
                issues = _issueProvider.GetAllIssuesByMilestoneThatHasTag(milestone.Number, _packageProvider.GetAllPackagesTags());

            return new Tuple<GithubMilestone, IList<GithubIssue>>(milestone, issues);
        }

        private List<ReleasePackage> MapCategories(IDictionary<string, GlimpsePackageGroup> packageCategories, IList<string> packageTags, IList<GithubIssue> issues)
        {
            return packageCategories.Select(x =>
            {
                var packageGroup = x.Value;
                var packageIssues = issues.Where(i => i.Labels.Any(l => packageGroup.Tags.Contains(l.Name))).Select(y => MapIssue(y, packageTags));

                return new ReleasePackage
                {
                    AcknowledgedIssues = packageIssues.Where(i => i.State == "open").ToList(),
                    CompletedIssues = packageIssues.Where(i => i.State == "closed").ToList(),
                    PackageItem = MapPackage(packageGroup.Packages),
                    Name = packageGroup.Name
                };
            }).ToList();
        }

        private List<ReleasePackageItem> MapPackage(List<GlimpsePackage> packages)
        {
            return packages.Select(x => new ReleasePackageItem
            {
                Name = x.Title,
                Status = x.Status,
                StatusDescription = x.StatusDescription
            }).ToList();
        }

        private List<Tuple<ReleaseUser, List<ReleaseIssue>>> MapPullRequestContributors(IList<GithubIssue> issues, IList<string> packageTags)
        { 
            var pullRequestContributors = new Dictionary<string, Tuple<GithubUser, List<GithubIssue>>>();
            var pullRequests = issues.Where(x => x.Pull_Request.Diff_Url != null);
            foreach (var pullRequest in pullRequests)
            {
                var record = (Tuple<GithubUser, List<GithubIssue>>)null;
                if (!pullRequestContributors.TryGetValue(pullRequest.User.Id, out record))
                {
                    record = new Tuple<GithubUser, List<GithubIssue>>(pullRequest.User, new List<GithubIssue>{pullRequest});
                    pullRequestContributors.Add(pullRequest.User.Id, record);
                }
                else
                    record.Item2.Add(pullRequest);
            }

            return pullRequestContributors.Values
                .Select(x => new Tuple<ReleaseUser, List<ReleaseIssue>>(MapUser(x.Item1), MapIssues(x.Item2, packageTags)))
                .ToList();
        }

        private ReleaseUser MapUser(GithubUser user)
        {
            return new ReleaseUser
            {
                AvatarUrl = user.Avatar_Url,
                HtmlUrl = user.Html_Url,
                Id = user.Id,
                Login = user.Login
            };
        }

        private ReleaseIssue MapIssue(GithubIssue issue, IList<string> packageTags)
        {
            return new ReleaseIssue
            {
                Category = string.Join(", ", issue.Labels.Where(x => !packageTags.Contains(x.Name)).Select(y => y.Name).ToArray()),
                Title = issue.Title,
                IssueId = issue.Id,
                IssueLinkUrl = issue.Html_Url,
                Number = issue.Number,
                User = MapUser(issue.User),
                State = issue.State
            };
        }

        private List<ReleaseIssue> MapIssues(IList<GithubIssue> issues, IList<string> packageTags)
        {
            return issues.Select(i => MapIssue(i, packageTags)).ToList();
        }

        private List<ReleaseUser> MapIssueReporters(IList<GithubIssue> issues)
        {
            return issues.Select(x => x.User)
                .DistinctBy(x => x.Id)
                .Select(MapUser)
                .ToList();
        }

        private ReleaseMilestone MapMilestone(GithubMilestone milestone)
        {
            var result = new ReleaseMilestone();
            if (milestone != null)
            {
                result.ClosedIssues = milestone.Closed_Issues;
                result.CreatedAt = milestone.Created_At;
                result.Description = milestone.Description;
                result.DueOn = milestone.Due_On;
                result.Number = milestone.Number;
                result.OpenIssues = milestone.Open_Issues;
                result.State = milestone.State;
                result.Title = milestone.Title;
                result.Url = milestone.Url;
            }

            return result;
        }
    } 
}
