using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Glimpse.Issues
{
    public class AuthenticatedHttpClient : IHttpClient
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly HttpClient _httpClient;

        public AuthenticatedHttpClient(string baseAddress, string mediaType, string clientId, string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _httpClient = new HttpClient() { BaseAddress = new Uri(baseAddress)};
            _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Glimpse.Site","v1.0"));
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            
        }
        public Task<HttpResponseMessage> GetAsync(string uri)
        {
            var uriBuilder = new UriBuilder(_httpClient.BaseAddress + uri);
            var query = uriBuilder.Uri.AddParameter("client_id", _clientId).AddParameter("client_secret", _clientSecret);
            return _httpClient.GetAsync(query);
        }
    }
}