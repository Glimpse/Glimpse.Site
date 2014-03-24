using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Glimpse.Twitter;
using WebApi.OutputCache.V2;

namespace Glimpse.Site.Areas.api.Controllers
{
    public class TwitterController : ApiController
    {
        private const int CacheTimeSpan = 30 * 60;

        [CacheOutput(ClientTimeSpan = CacheTimeSpan, ServerTimeSpan = CacheTimeSpan)]
        public async Task<HttpResponseMessage> GetLatest()
        {
            var tweets = await TwitterSettings.Settings.TweetQueryProvider.LatestWithGlimpse();

            var sc = new StringContent(tweets);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var resp = new HttpResponseMessage { Content = sc };

            return resp; 
        }
    }
}
