using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Glimpse.Release.Support;

namespace Glimpse.Release
{
    public class MilestoneProvider : IMilestoneProvider
    {
        private readonly IHttpClient _httpClient;
        private IList<GithubMilestone> _milestones;

        protected IList<GithubMilestone> Milestones
        {
            get { return _milestones ?? (_milestones = InnerGetAllMilestones()); }
        }

        public MilestoneProvider(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public GithubMilestone GetMilestone(string title)
        {
            return Milestones.FirstOrDefault(g => g.Title.ToLower() == title.ToLower());
        }

        public GithubMilestone GetLatestMilestoneWithIssues(string state)
        {
            return (from g in Milestones
                    where g.State == state && (g.Open_Issues > 0 || g.Closed_Issues > 0)
                    orderby g.Created_At descending
                    select g).First();
        }

        public IList<GithubMilestone> GetAllMilestones()
        {
            return Milestones;
        }

        public void Clear()
        {
            _milestones = null;
        }

        private IList<GithubMilestone> InnerGetAllMilestones()
        {
            var data = new List<GithubMilestone>();

            var result1 = _httpClient.GetPagedDataAsync<GithubMilestone>(new Uri("https://api.github.com/repos/glimpse/glimpse/milestones"));
            var result2 = _httpClient.GetPagedDataAsync<GithubMilestone>(new Uri("https://api.github.com/repos/glimpse/glimpse/milestones?state=closed"));

            data.AddRange(result1);
            data.AddRange(result2);

            return result1;
        }
    }
}
