using System;
using System.Collections.Generic;

namespace Glimpse.Release
{
    public class Release
    {
        public Release()
        {
            PackageCategories = new List<ReleasePackage>();
            IssueReporters = new List<ReleaseUser>();
            PullRequestContributors = new List<Tuple<ReleaseUser, List<ReleaseIssue>>>();
        }

        public List<ReleasePackage> PackageCategories { get; set; }

        public List<ReleaseUser> IssueReporters { get; set; }

        public List<Tuple<ReleaseUser, List<ReleaseIssue>>> PullRequestContributors { get; set; }

        public ReleaseMilestone Milestone { get; set; }
    }
}