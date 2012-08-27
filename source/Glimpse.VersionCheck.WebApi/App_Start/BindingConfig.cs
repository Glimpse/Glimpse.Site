using System.Web.Http;
using System.Web.Mvc;
using Glimpse.VersionCheck.WebApi.Controllers;
using Glimpse.VersionCheck.WebApi.Framework;

namespace Glimpse.VersionCheck.WebApi
{
    public class BindingConfig
    {
        public static void RegisterGlobalBindings(ModelBinderDictionary binders, HttpConfiguration configuration)
        { 
            var provider = new VersionCheckDetailsApiModelBinderProvider();

            ModelBinders.Binders.Add(typeof(VersionCheckDetails), provider.VersionCheckDetailsModelBinder);
            configuration.Services.Insert(typeof(System.Web.Http.ModelBinding.ModelBinderProvider), 0, provider);
        }
    }
}