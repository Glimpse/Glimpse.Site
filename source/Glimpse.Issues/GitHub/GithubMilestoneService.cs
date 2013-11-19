using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Glimpse.Issues
{
    public class GithubMilestoneService : IGithubMilestoneService
    {
        private readonly IHttpClient _httpClient;

        public GithubMilestoneService(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public GithubMilestone GetLatestMilestoneWithIssues()
        {
            var githubMilestones = GetAllMilestones();
            var vnextMilestone = githubMilestones.FirstOrDefault(g => g.Title.ToLower() == "vnext");
            if (MileStoneHasOpenOrClosedIssues(vnextMilestone))
                return vnextMilestone;
            return (from g in githubMilestones
                    where g.Open_Issues > 0 || g.Closed_Issues > 0
                    orderby g.Created_At descending
                    select g).First();
        }

        private static bool MileStoneHasOpenOrClosedIssues(GithubMilestone vnextMilestone)
        {
            return vnextMilestone != null && vnextMilestone.Closed_Issues != 0 && vnextMilestone.Open_Issues != 0;
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
