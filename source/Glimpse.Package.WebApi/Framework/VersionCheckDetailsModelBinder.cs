using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace Glimpse.Package.WebApi.Framework
{
    public class VersionCheckDetailsModelBinder : IModelBinder, System.Web.Http.ModelBinding.IModelBinder
    {
        private IDictionary<string, int> _reservedKeys = new Dictionary<string, int>{{"stamp", 0}, {"callback", 0}, {"_", 0}};

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
            var stamp = queryString["stamp"];

            var model = new VersionCheckDetails();
            var items = new List<VersionCheckDetailsItem>();

            if (queryString.AllKeys.Length > 1)
            {
                foreach (var token in queryString.AllKeys)
                {
                    if (!_reservedKeys.ContainsKey(token))
                        items.Add(new VersionCheckDetailsItem { Name = token, Version = queryString[token] });
                }
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