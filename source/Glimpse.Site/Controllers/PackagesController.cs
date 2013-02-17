using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glimpse.Package;

namespace Glimpse.Site.Controllers
{
    public partial class PackagesController : Controller
    {
        public virtual ActionResult Index()
        {
            var packages = PackageSettings.Settings.QueryProvider.SelectAllPackages();
            var result = packages.Select(keyValue => keyValue.Value.FirstOrDefault(value => value.IsAbsoluteLatestVersion)).Where(x => x != null).OrderBy(x => x.Name).ToList();

            return View(result);
        } 
    }
}
