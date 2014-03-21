using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace Glimpse.Site.Controllers
{
    public partial class HomeController : Controller
    {
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult GettingStarted(string task)
        {
            return View(task);
        } 

        [OutputCache(Duration = 3600)] // Cache for 1 hour
        public virtual async Task<ActionResult> TweetsLatest()
        {
            var key = ConfigurationManager.AppSettings["TwitterKey"];
            var secret = ConfigurationManager.AppSettings["TwitterSecret"];
            var bearerToken = GetBearerTokenCredentials(key, secret);

            var httpClient = new HttpClient();

            var accessTokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://api.twitter.com/oauth2/token");
            accessTokenRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", bearerToken);
            accessTokenRequest.Headers.Accept.ParseAdd("application/x-www-form-urlencoded;charset=UTF-8");
            accessTokenRequest.Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "grant_type", "client_credentials" }
                });

            var accessTokenResponse = await httpClient.SendAsync(accessTokenRequest);
            accessTokenResponse.EnsureSuccessStatusCode();
            var jsonString = await accessTokenResponse.Content.ReadAsStringAsync();
            dynamic json = JObject.Parse(jsonString);
            string accessToken = json.access_token;

            var query = "from:@nikmd23 OR from:@anthony_vdh OR from:@CGijbels #glimpse";
            var searchRequest = new HttpRequestMessage(HttpMethod.Get, "https://api.twitter.com/1.1/search/tweets.json?q=" + HttpUtility.UrlEncode(query));
            searchRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var searchResponse = await httpClient.SendAsync(searchRequest);
            searchResponse.EnsureSuccessStatusCode();
            var result = await searchResponse.Content.ReadAsStringAsync();

            return Content(result, "application/json");
        }

        [OutputCache(Duration = 3600)] // Cache for 1 hour
        public async virtual Task<ActionResult> BuildLatest()
        {
            var httpClient = new HttpClient();

            var response = await httpClient.GetStringAsync("http://teamcity.codebetter.com/app/rest/builds/buildType:%28id:bt428%29?guest=1");
            var xml = XElement.Parse(response);

            DateTimeOffset date;
            DateTimeOffset.TryParseExact(xml.Element("startDate").Value, "yyyyMMddTHHmmsszz00", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

            return Json(
                new
                {
                    id = xml.Attribute("id").Value,
                    number = xml.Attribute("number").Value,
                    status = xml.Attribute("status").Value.ToLower(),
                    link = xml.Attribute("webUrl").Value + "&guest=1",
                    date = date.DateTime.ToShortDateString(),
                    time = date.DateTime.ToShortTimeString()
                },
                JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 3600)] // Cache for 1 hour
        public async virtual Task<ActionResult> BlogLatest()
        {
            var httpClient = new HttpClient();

            var response = await httpClient.GetStringAsync("http://feeds.getglimpse.com/getglimpse");
            var xml = XElement.Parse(response);
            var result = new List<object>();

            foreach (var item in xml.Descendants("item").Take(2))
            {
                result.Add(new
                {
                    title = item.Element("title").Value,
                    summary = item.Element("description").Value,
                    link = item.Element("link").Value
                });
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private static string GetBearerTokenCredentials(string key, string secret)
        {
            var encodedKey = HttpUtility.UrlEncode(key);
            var encodedSecret = HttpUtility.UrlEncode(secret);

            var concatenatedKeySecret = string.Format("{0}:{1}", encodedKey, encodedSecret);

            return Convert.ToBase64String(Encoding.ASCII.GetBytes(concatenatedKeySecret));
        }
    }
}