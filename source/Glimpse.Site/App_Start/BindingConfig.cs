using System.Web.Http;
using System.Web.Mvc; 
using Glimpse.Package;
using Glimpse.Site.Framework;

namespace Glimpse.Site
{
    public class BindingConfig
    {
        public static void RegisterGlobalBindings(ModelBinderDictionary binders, HttpConfiguration configuration)
        { 
            ModelBinders.Binders.Add(typeof(VersionCheckDetails), new VersionCheckDetailsModelBinder()); 
        }
    }
}