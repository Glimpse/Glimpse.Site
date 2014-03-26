using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Glimpse.Release.Support
{
    public class BasicHttpClient : BaseHttpClient
    {
        public BasicHttpClient(string baseAddress, string mediaType)
            : base(baseAddress, mediaType)
        {
        }

        public override Task<HttpResponseMessage> GetAsync(string uri)
        {
            var uriBuilder = new UriBuilder(uri);
            return _httpClient.GetAsync(uriBuilder.Uri);
        }
    }
}