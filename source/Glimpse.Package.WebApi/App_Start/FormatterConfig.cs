using System.Net.Http.Headers;
using System.Web.Http;
using Glimpse.Package.WebApi.Controllers;
using Glimpse.Package.WebApi.Framework;
using Newtonsoft.Json.Serialization;

namespace Glimpse.Package.WebApi
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