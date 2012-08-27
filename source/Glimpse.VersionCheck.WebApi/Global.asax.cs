using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Mvc;
using System.Web.Routing;
using Glimpse.VersionCheck.WebApi.Controllers;
using Newtonsoft.Json.Serialization;

namespace Glimpse.VersionCheck.WebApi
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlimpseSettings.Settings.Initialize();

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BindingConfig.RegisterGlobalBindings(ModelBinders.Binders, GlobalConfiguration.Configuration);
            FormatterConfig.RegisterRoutes(GlobalConfiguration.Configuration);

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new DictionaryKeysAreNotPropertyNamesJsonConverter());
        }
    }
}