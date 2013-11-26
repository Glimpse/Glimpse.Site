using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Glimpse.Issues
{
    public class GithubMilestoneService : IGithubMilestoneService
    {
        private readonly IHttpClient _httpClient;
        private List<GithubMilestone> _githubMilestones;

        protected List<GithubMilestone> GithubMilestones
        {
            get { return _githubMilestones ?? (_githubMilestones = GetAllMilestones()); }
        }

        public GithubMilestoneService(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public GithubMilestone GetMilestone(string title)
        {
            return GithubMilestones.FirstOrDefault(g => g.Title.ToLower() == title.ToLower());
        }

        public GithubMilestone GetLatestMilestoneWithIssues(string state)
        {
            return (from g in GithubMilestones
                    where g.State == state && (g.Open_Issues > 0 || g.Closed_Issues > 0)
                    orderby g.Created_At descending
                    select g).First();
        }

        private List<GithubMilestone> GetAllMilestones()
        {
            var result = _httpClient.GetAsync("repos/glimpse/glimpse/milestones").Result;
            
            var githubMilestones = result.Content.ReadAsAsync<IEnumerable<GithubMilestone>>().Result.ToList();
            result = _httpClient.GetAsync("repos/glimpse/glimpse/milestones?state=closed").Result;
            var closedMilestones = result.Content.ReadAsAsync<IEnumerable<GithubMilestone>>().Result.ToList();
            githubMilestones.AddRange(closedMilestones);
            return githubMilestones;

        }
    }
}
