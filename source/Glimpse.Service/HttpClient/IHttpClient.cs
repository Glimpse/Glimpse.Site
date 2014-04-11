using System.Net.Http;
using System.Threading.Tasks;

namespace Glimpse.Service
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string uri);
    }
}