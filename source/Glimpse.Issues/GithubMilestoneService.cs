using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Hosting;

namespace Glimpse.Issues
{
    public class GithubMilestoneService
    {
        public GithubMilestone GetMilestone(string milestoneName)
        {
            var client = SetupHttpClient("https://api.github.com/", "application/json");
            var result = client.GetAsync("repos/glimpse/glimpse/milestones").Result;
            var path = HostingEnvironment.MapPath("/Content/api.txt");
            File.AppendAllText(path, string.Format("{0:dd/MM/yyyy HH:mm:ss} - {1} - {2}\n", DateTime.UtcNow, "milestones", result.Content.ReadAsStringAsync().Result));
            var milestones = result.Content.ReadAsAsync<IEnumerable<GithubMilestone>>().Result;
            return milestones.FirstOrDefault(m => m.Title.ToLower() == milestoneName.ToLower());
        }

        private HttpClient SetupHttpClient(string baseAddress, string mediaType)
        {
            var client = new HttpClient {BaseAddress = new Uri(baseAddress)};
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            return client;
        }
    }
}
