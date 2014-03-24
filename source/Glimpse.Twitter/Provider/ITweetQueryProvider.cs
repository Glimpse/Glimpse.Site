using System.Collections.Generic;
using System.Threading.Tasks;

namespace Glimpse.Twitter
{
    public interface ITweetQueryProvider
    {
        Task<string> LatestWithGlimpse();
    }
}