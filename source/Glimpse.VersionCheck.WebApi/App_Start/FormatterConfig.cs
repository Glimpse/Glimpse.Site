using System.Web.Http;
using Glimpse.VersionCheck.WebApi.Controllers;
using Glimpse.VersionCheck.WebApi.Framework;

namespace Glimpse.VersionCheck.WebApi
{
    public class FormatterConfig
    {
        public static void RegisterRoutes(HttpConfiguration configuration)
        {
            configuration.Formatters.Insert(0, new JsonpMediaTypeFormatter());
        }
    }
}