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
using Glimpse.Site.Framework;
using WebApi.OutputCache.V2;

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
            var httpClient = HttpClientHelper.CreateGithub(githubKey, githubSecret);
            var contributorService = new ContributorService(new GlimpseTeamMemberRepository(teamMemberJsonFile),new GithubContributorService(httpClient));
            return contributorService.GetContributors().Take(string.IsNullOrEmpty(top) ? 11 : int.Parse(top));
        }
    }
}
