using System.Web.Mvc;
using Glimpse.Site.Models;

namespace Glimpse.Site.Controllers
{
    public class DocsController : Controller
    {
        public ActionResult Index(string mdSlug, string controller = null)
        {
            return View(new DocumentationViewModel(mdSlug, "Views/" + controller + "/Wiki/"));
        }
    }
}