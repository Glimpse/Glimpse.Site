using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http.ModelBinding;
using System.Web.Mvc;

namespace Glimpse.VersionCheck.WebApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}