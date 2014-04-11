using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Glimpse.Blog;
using WebApi.OutputCache.V2;

namespace Glimpse.Site.Areas.api.Controllers
{
    public class BlogController : ApiController
    {
        private const int CacheTimeSpan = 30 * 60;

        [CacheOutput(ClientTimeSpan = CacheTimeSpan, ServerTimeSpan = CacheTimeSpan)]
        public async Task<List<BlogResult>> GetLatest()
        {
            var status = await BlogSettings.Settings.PostQueryProvider.CurrentPosts();

            return status;
        }
    }
}
