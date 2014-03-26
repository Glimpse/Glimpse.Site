using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Glimpse.Release.Support
{
    public class AuthenticatedHttpClient : BaseHttpClient
    {
        private readonly string _clientId;
        private readonly string _clientSecret;

        public AuthenticatedHttpClient(string baseAddress, string mediaType, string clientId, string clientSecret)
            : base(baseAddress, mediaType)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        public override Task<HttpResponseMessage> GetAsync(string uri)
        {
            var uriBuilder = new UriBuilder(uri);
            var query = uriBuilder.Uri.SetParameter("client_id", _clientId).SetParameter("client_secret", _clientSecret);

            return _httpClient.GetAsync(query);
        }
    }
}