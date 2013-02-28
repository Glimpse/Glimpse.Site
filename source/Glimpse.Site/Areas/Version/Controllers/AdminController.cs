using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc; 
using Glimpse.Package;

namespace Glimpse.Site.Areas.Version.Controllers
{
    public partial class AdminController : Controller
    {
        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ActionResult Process()
        {
            var service = PackageSettings.Settings.RefreshReleaseService;
            var results = service.Execute();

            return View(results);
        }
    }
}
