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

        public IEnumerable<GithubMilestone> GetMilestones()
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
