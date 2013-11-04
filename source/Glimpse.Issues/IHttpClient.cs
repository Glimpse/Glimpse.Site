using System.Net.Http;
using System.Threading.Tasks;

namespace Glimpse.Issues
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string uri);
    }
}