using System;
using System.Configuration;

namespace Glimpse.Service
{
    public class HttpClientFactory
    {
        public static IHttpClient CreateGithub()
        {
            var githubKey = ConfigurationManager.AppSettings.Get("GithubKey");
            var githubSecret = ConfigurationManager.AppSettings.Get("GithubSecret");

            const string baseAddress = "https://api.github.com/";
            const string mediaType = "application/json";

            if (githubKey == null || githubSecret == null || githubKey == githubSecret)
                return new BasicHttpClient(baseAddress, mediaType);

            return new AuthenticatedHttpClient(baseAddress, mediaType, githubKey, githubSecret);
        }
    }
}
