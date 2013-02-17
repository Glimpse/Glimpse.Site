using System.Web.Http;
using System.Web.Mvc;
using Glimpse.Package.WebApi.Controllers;
using Glimpse.Package.WebApi.Framework;

namespace Glimpse.Package.WebApi
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