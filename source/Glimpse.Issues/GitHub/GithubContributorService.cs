using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Glimpse.Infrastructure.Http;
using Glimpse.Infrastructure.Models;
using Glimpse.Infrastructure.Services;

namespace Glimpse.Infrastructure.GitHub
{
    public class GithubContributorService : IGithubContributerService
    {
        private readonly IHttpClient _httpClient;

        public GithubContributorService(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IEnumerable<GithubContributor> GetContributors(string githubRepoName)
        {
            var result = _httpClient.GetAsync("repos/" + githubRepoName + "/contributors").Result;
            return Enumerable.ToList<GithubContributor>(result.Content.ReadAsAsync<IEnumerable<GithubContributor>>().Result);
        }
    }
}