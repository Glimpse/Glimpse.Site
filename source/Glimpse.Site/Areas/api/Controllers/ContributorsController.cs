using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Hosting;
using System.Web.Http;
using Glimpse.Infrastructure.GitHub;
using Glimpse.Infrastructure.Models;
using Glimpse.Infrastructure.Repositories;
using Glimpse.Infrastructure.Services;
using WebAPI.OutputCache;

namespace Glimpse.Site.Areas.api.Controllers
{
    public class ContributorsController : ApiController
    {
        private const int CacheTimeSpan = 30 * 60;

        [CacheOutput(ClientTimeSpan = CacheTimeSpan, ServerTimeSpan = CacheTimeSpan)]
        public IEnumerable<GlimpseContributor> Get(string top = "")
        {
            string teamMemberJsonFile = HostingEnvironment.MapPath("~/Content/glimpseTeam.json");
            string githubKey = ConfigurationManager.AppSettings.Get("GithubKey");
            string githubSecret = ConfigurationManager.AppSettings.Get("GithubSecret");
            var httpClient = new HttpClientFactory().CreateHttpClient(githubKey, githubSecret);
            var contributorService = new ContributorService(new GlimpseTeamMemberRepository(teamMemberJsonFile),new GithubContributorService(httpClient));
            return contributorService.GetContributors().Take(string.IsNullOrEmpty(top) ? 11 : int.Parse(top));
        }

    }
}
