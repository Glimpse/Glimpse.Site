using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Glimpse.Release.Support
{
    public abstract class BaseHttpClient : IHttpClient
    {
        protected readonly HttpClient _httpClient;

        protected BaseHttpClient(string baseAddress, string mediaType)
        {
            _httpClient = new HttpClient() { BaseAddress = new Uri(baseAddress) };
            _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Glimpse.Site", "v1.0"));
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
        }

        public abstract Task<HttpResponseMessage> GetAsync(string uri);
    }
}
