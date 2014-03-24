using System.Collections.Generic;
using System.Threading.Tasks;

namespace Glimpse.Build
{
    public interface IStatusQueryProvider
    {
        Task<StatusResult> CurrentStatus();
    }
}