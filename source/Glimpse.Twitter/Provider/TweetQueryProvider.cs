using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;

namespace Glimpse.Twitter
{
    public class TweetQueryProvider : ITweetQueryProvider
    {
        public async Task<string> LatestWithGlimpse()
        {
            var accessToken = await GetAccessToken();

            var query = "from:@nikmd23 OR from:@anthony_vdh OR from:@CGijbels #glimpse";
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.twitter.com/1.1/search/tweets.json?q=" + HttpUtility.UrlEncode(query));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await new HttpClient().SendAsync(request);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();

            return jsonString;
        }

        private static async Task<string> GetAccessToken()
        {
            var bearerToken = GetAccessTokenCredentials();

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.twitter.com/oauth2/token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", bearerToken);
            request.Headers.Accept.ParseAdd("application/x-www-form-urlencoded;charset=UTF-8");
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string> { { "grant_type", "client_credentials" } });
             
            var response = await new HttpClient().SendAsync(request);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            dynamic json = JObject.Parse(jsonString);

            return json.access_token;
        }

        private static string GetAccessTokenCredentials()
        {
            var key = ConfigurationManager.AppSettings["TwitterKey"];
            var secret = ConfigurationManager.AppSettings["TwitterSecret"];

            var encodedKey = HttpUtility.UrlEncode(key);
            var encodedSecret = HttpUtility.UrlEncode(secret);

            var concatenatedKeySecret = string.Format("{0}:{1}", encodedKey, encodedSecret);

            return Convert.ToBase64String(Encoding.ASCII.GetBytes(concatenatedKeySecret));
        }
    }
}