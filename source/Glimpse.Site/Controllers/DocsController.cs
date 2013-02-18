using System.Web.Mvc;

namespace Glimpse.Site.Controllers
{
    public class DocsController : Controller
    {
        public ActionResult Index(string mdSlug, string controller = null)
        {
            //if (string.IsNullOrEmpty(mdSlug))
                return Content("Help placeholder!");
            // TODO : Provide real content!

            //return View(new DocumentationViewModel(mdSlug, "Views/" + controller + "/Wiki/"));
        }
    }
}