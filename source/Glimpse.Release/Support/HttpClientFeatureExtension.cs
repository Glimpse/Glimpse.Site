using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Glimpse.Release.Support
{
    public static class HttpClientFeatureExtension
    {
        public static List<T> GetPagedDataAsync<T>(this IHttpClient httpClient, Uri address)
        {
            var store = new List<T>();
            address = address.SetParameter("per_page", "100");

            var nextUri = address;
            while (nextUri != null)
            {
                var response = httpClient.GetAsync(nextUri.ToString()).Result;
                var result = response.Content.ReadAsAsync<List<T>>().Result;

                store.AddRange(result);

                nextUri = GetNextPage(response.Headers);
            }

            return store;
        }

        private static Uri GetNextPage(HttpResponseHeaders headers)
        {
            if (headers.Contains("Link"))
            {
                var link = headers.GetValues("Link").First();
                var rawUri = link.Split(',')
                    .Select(x => x.Trim())
                    .Where(x => x.EndsWith("rel=\"next\""))
                    .Select(x => x.Split(';').First().Trim().Replace("<", "").Replace(">", "")).FirstOrDefault();
                if (rawUri != null)
                {
                    return new Uri(rawUri);
                }
            }

            return null;
        }
    }
}