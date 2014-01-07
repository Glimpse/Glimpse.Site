using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using Glimpse.Infrastructure.GitHub;
using Glimpse.Infrastructure.Http;
using Glimpse.Infrastructure.Models;
using Glimpse.Infrastructure.Repositories;
using Glimpse.Infrastructure.Services;

namespace Glimpse.Site.Areas.api.Controllers
{
    public class ContributorsController : ApiController
    {
        // GET api/contributors
        public IEnumerable<GlimpseContributor> Get()
        {
            string teamMemberJsonFile = HostingEnvironment.MapPath("~/Content/glimpseTeam.json");
            string githubKey = ConfigurationManager.AppSettings.Get("GithubKey");
            string githubSecret = ConfigurationManager.AppSettings.Get("GithubSecret");
            var httpClient = new HttpClientFactory().CreateHttpClient(githubKey, githubSecret);
            var contributorService = new ContributorService(new GlimpseTeamMemberRepository(teamMemberJsonFile),new GithubContributorService(httpClient));
            return contributorService.GetContributors();
        }

        // GET api/contributors/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/contributors
        public void Post([FromBody]string value)
        {
        }

        // PUT api/contributors/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/contributors/5
        public void Delete(int id)
        {
        }
    }

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
            return result.Content.ReadAsAsync<IEnumerable<GithubContributor>>().Result.ToList();
        }
    }
}
