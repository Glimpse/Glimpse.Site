using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace Glimpse.Site.Framework
{
    public class HttpHeaderAttribute : ActionFilterAttribute 
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public HttpHeaderAttribute(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Response.Headers.Add(Name, Value);
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}