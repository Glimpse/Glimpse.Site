using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace Glimpse.VersionCheck.WebApi.Framework
{
    public class VersionCheckDetailsModelBinder : IModelBinder, System.Web.Http.ModelBinding.IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, System.Web.Http.ModelBinding.ModelBindingContext bindingContext)
        {
            var queryString = HttpUtility.ParseQueryString(actionContext.Request.RequestUri.Query);

            bindingContext.Model = BuildModel(queryString);

            return true;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var queryString = controllerContext.HttpContext.Request.QueryString;

            return BuildModel(queryString);
        } 

        private object BuildModel(NameValueCollection queryString)
        {
            var names = PraseList(queryString["packages"]);
            var versions = PraseList(queryString["versions"]);
            var stamp = queryString["stamp"];

            var model = new VersionCheckDetails();
            var items = new List<VersionCheckDetailsItem>();

            if (names.Length == versions.Length)
            {
                for (var i = 0; i < names.Length; i++)
                    items.Add(new VersionCheckDetailsItem { Name = names[i], Version = versions[i] });
            }

            model.Packages = items;
            model.Stamp = stamp;

            return model;
        }

        private string[] PraseList(string value)
        {
            return string.IsNullOrEmpty(value) ? new string[0] : value.Split(',');
        }
    }
}