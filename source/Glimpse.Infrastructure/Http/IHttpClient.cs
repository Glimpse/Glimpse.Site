using System.Net.Http;
using System.Threading.Tasks;

namespace Glimpse.Infrastructure.Http
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string uri);
    }
}