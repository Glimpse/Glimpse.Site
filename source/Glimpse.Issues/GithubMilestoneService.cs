using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Glimpse.Issues
{
    public class GithubMilestoneService
    {
        private readonly IHttpClient _httpClient;

        public GithubMilestoneService(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public GithubMilestone GetMilestone(string milestoneName)
        {
            var result = _httpClient.GetAsync("repos/glimpse/glimpse/milestones").Result;
            var milestones = result.Content.ReadAsAsync<IEnumerable<GithubMilestone>>().Result;
            return milestones.FirstOrDefault(m => m.Title.ToLower() == milestoneName.ToLower());
        }
    }
}
