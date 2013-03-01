using System.Web.Mvc;
using Glimpse.Site.Models;

namespace Glimpse.Site.Controllers
{
    public partial class DocsController : Controller
    {
        public virtual ActionResult Index(string mdSlug, string controller = null)
        {
            if (string.IsNullOrEmpty(mdSlug))
                mdSlug = "Getting-Started";

            return View(new DocumentationViewModel(mdSlug, "Views/" + controller + "/Wiki/Content/"));
        }
    }
}