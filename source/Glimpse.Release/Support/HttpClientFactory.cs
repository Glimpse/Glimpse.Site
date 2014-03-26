using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.Release.Support
{
    public class HttpClientFactory
    {
        public static IHttpClient CreateGithub(string githubKey, string githubSecret)
        {
            const string baseAddress = "https://api.github.com/";
            const string mediaType = "application/json";

            if (githubKey == null || githubSecret == null || githubKey == githubSecret)
                return new BasicHttpClient(baseAddress, mediaType);

            return new AuthenticatedHttpClient(baseAddress, mediaType, githubKey, githubSecret);
        }
    }
}
