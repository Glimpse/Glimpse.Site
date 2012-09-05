using System.Net.Http.Headers;
using System.Web.Http;
using Glimpse.VersionCheck.WebApi.Controllers;
using Glimpse.VersionCheck.WebApi.Framework;
using Newtonsoft.Json.Serialization;

namespace Glimpse.VersionCheck.WebApi
{
    public class FormatterConfig
    {
        public static void RegisterFormatters(HttpConfiguration configuration)
        {
            configuration.Formatters.Insert(0, new JsonpMediaTypeFormatter());

            configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new DictionaryKeysAreNotPropertyNamesJsonConverter());

            configuration.Formatters[0].SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }
    }
}