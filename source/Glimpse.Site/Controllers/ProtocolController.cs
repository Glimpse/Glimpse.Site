using System.Web.Mvc;

namespace Glimpse.Site.Controllers
{
    public partial class ProtocolController : Controller
    {
        public virtual ActionResult Index()
        {
            return View();
        }
    }
}