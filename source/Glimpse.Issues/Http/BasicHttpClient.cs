using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace Glimpse.Issues
{
    public class BasicHttpClient : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public BasicHttpClient(string baseAddress, string mediaType)
        {
            _httpClient = new HttpClient() {BaseAddress = new Uri(baseAddress)};
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
        }

        public Task<HttpResponseMessage> GetAsync(string uri)
        {
            return _httpClient.GetAsync(uri);
        }
    }

    public class AuthenticatedHttpClient : IHttpClient
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly HttpClient _httpClient;

        public AuthenticatedHttpClient(string baseAddress, string mediaType, string clientId, string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _httpClient = new HttpClient() { BaseAddress = new Uri(baseAddress) };
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            
        }
        public Task<HttpResponseMessage> GetAsync(string uri)
        {
            var uriBuilder = new UriBuilder(_httpClient.BaseAddress + uri);
            var query = uriBuilder.Uri.AddParameter("client_id", _clientId).AddParameter("client_secret", _clientSecret);
            return _httpClient.GetAsync(query);
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