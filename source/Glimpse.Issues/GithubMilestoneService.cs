using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Glimpse.Issues
{
    public class GithubMilestoneService
    {
        public GithubMilestone GetMilestone(string milestoneName)
        {
            var client = SetupHttpClient("https://api.github.com/", "application/json");
            var result = client.GetAsync("repos/glimpse/glimpse/milestones").Result;
            var milestones = result.Content.ReadAsAsync<IEnumerable<GithubMilestone>>().Result;
            return milestones.First(m => m.Title.ToLower() == milestoneName.ToLower());
        }

        private HttpClient SetupHttpClient(string baseAddress, string mediaType)
        {
            var client = new HttpClient {BaseAddress = new Uri(baseAddress)};
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            return client;
        }
    }
}