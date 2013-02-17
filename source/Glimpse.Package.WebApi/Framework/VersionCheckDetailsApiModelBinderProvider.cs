using System;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace Glimpse.Package.WebApi.Framework
{
    public class VersionCheckDetailsApiModelBinderProvider : ModelBinderProvider
    {
        private readonly VersionCheckDetailsModelBinder _modelBinder = new VersionCheckDetailsModelBinder();

        public VersionCheckDetailsModelBinder VersionCheckDetailsModelBinder { get { return _modelBinder; } }

        public override IModelBinder GetBinder(HttpConfiguration configuration, Type modelType)
        {
            if (modelType == typeof(VersionCheckDetails))
                return _modelBinder;
            return null;
        }
    }
}