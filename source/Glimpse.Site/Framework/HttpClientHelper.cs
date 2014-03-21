using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glimpse.Infrastructure.Http;

namespace Glimpse.Site.Framework
{
    public class HttpClientHelper
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