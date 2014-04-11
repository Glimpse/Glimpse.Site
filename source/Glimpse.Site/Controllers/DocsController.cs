using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glimpse.Site.Models;

namespace Glimpse.Site.Controllers
{
    public partial class DocsController : Controller
    {
        public virtual ActionResult Index(string controller)
        {
            return View(BuildViewModel(controller));
        }

        public virtual ActionResult Details(string mdSlug, string controller = null)
        {
            return View(BuildViewModel(controller, mdSlug));
        }

        private DocumentationViewModel BuildViewModel(string controller, string mdSlug = null)
        {
            if (string.IsNullOrEmpty(mdSlug))
                mdSlug = "Getting-Started";

            return new DocumentationViewModel(mdSlug, "Views/" + controller + "/Wiki/Content/");
        }
	}
}