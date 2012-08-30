using System;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace Glimpse.VersionCheck.WebApi.Framework
{
    public class VersionCheckDetailsApiModelBinderProvider : System.Web.Http.ModelBinding.ModelBinderProvider
    {
        private readonly VersionCheckDetailsModelBinder _modelBinder = new VersionCheckDetailsModelBinder();

        public VersionCheckDetailsModelBinder VersionCheckDetailsModelBinder { get { return _modelBinder; } }

        /*
        public override System.Web.Http.ModelBinding.IModelBinder GetBinder(HttpActionContext actionContext, System.Web.Http.ModelBinding.ModelBindingContext bindingContext)
        { 
            if (bindingContext.ModelType == typeof(VersionCheckDetails))
                return _modelBinder;
            return null;
        }
        */

        public override IModelBinder GetBinder(HttpConfiguration configuration, Type modelType)
        {
            if (modelType == typeof(VersionCheckDetails))
                return _modelBinder;
            return null;
        }
    }
}