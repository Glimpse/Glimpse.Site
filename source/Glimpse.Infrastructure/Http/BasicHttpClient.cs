using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace Glimpse.Infrastructure.Http
{
    public class BasicHttpClient : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public BasicHttpClient(string baseAddress, string mediaType)
        {
            _httpClient = new HttpClient() {BaseAddress = new Uri(baseAddress)};
            _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Glimpse.Site","v1.0"));
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
        }

        public Task<HttpResponseMessage> GetAsync(string uri)
        {
            return _httpClient.GetAsync(uri);
        }
    }

    public static class UriExtensions
    {
        public static Uri AddParameter(this Uri url, string paramName, string paramValue)
        {
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query.Add(paramName, paramValue);
            uriBuilder.Query = query.ToString();

            return new Uri(uriBuilder.ToString());
        }
    }
}