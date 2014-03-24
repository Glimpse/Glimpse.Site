using System.Threading.Tasks;
using System.Web.Http;
using Glimpse.Build;
using WebApi.OutputCache.V2;

namespace Glimpse.Site.Areas.api.Controllers
{
    public class BuildController : ApiController
    { 
        private const int CacheTimeSpan = 30 * 60;

        [CacheOutput(ClientTimeSpan = CacheTimeSpan, ServerTimeSpan = CacheTimeSpan)]
        public async Task<StatusResult> GetStatus()
        {
            var status = await BuildSettings.Settings.StatusQueryProvider.CurrentStatus();
             
            return status;
        }
    }
}
