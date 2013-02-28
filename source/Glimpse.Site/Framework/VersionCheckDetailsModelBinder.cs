using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using Glimpse.Package;

namespace Glimpse.Site.Framework
{
    public class VersionCheckDetailsModelBinder : IModelBinder, System.Web.Http.ModelBinding.IModelBinder
    {
        private IDictionary<string, int> _reservedKeys = new Dictionary<string, int> { { "stamp", 0 }, { "callback", 0 }, { "_", 0 } };

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

            if (queryString.AllKeys.Length > 0)
            {
                foreach (var token in queryString.AllKeys)
                {
                    if (!_reservedKeys.ContainsKey(token))
                    {
                        var item = new VersionCheckDetailsItem { Name = token };
                        ParseVersion(item, queryString[token]);
                        items.Add(item);
                    }
                }
            }

            model.Packages = items;
            model.Stamp = stamp;

            return model;
        }

        private void ParseVersion(VersionCheckDetailsItem item, string version)
        {
            item.Version = version;
            item.VersionRange = version;
            if (!string.IsNullOrEmpty(version) && version.Contains(".."))
            {
                var split = version.Split(new[] { ".." }, StringSplitOptions.None);
                if (split.Length >= 1)
                    item.Version = split[0];
                if (split.Length >= 2)
                    item.VersionRange = split[1];
            }
        } 
    }
}