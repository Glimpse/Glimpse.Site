using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glimpse.Issues;
using Microsoft.Ajax.Utilities;

namespace Glimpse.Site.Models
{
    public class IssuesIndexViewModel
    {
        public IssuesIndexViewModel()
        {
            PackageCategories = new List<PackageCategoryViewModel>();    
        }

        public List<PackageCategoryViewModel> PackageCategories { get; set; }
        public List<GithubUser> IssueReporters { get; set; }
        public List<Tuple<GithubUser, List<GithubIssue>>> PullRequestContributors { get; set; }
        public GithubMilestone CurrentMilestone { get; set; }

    }

}