using System.Web.Mvc;

namespace Glimpse.Site.Controllers
{
    public partial class HomeController : Controller
    {
        public virtual ActionResult Index()
        {
            return View(MVC.Home.Views.Index, MVC.Shared.Views._Home);
        }

        public virtual ActionResult Support()
        {
            return View();
        }

        public virtual ActionResult Protocol()
        {
            return View();
        }

        public virtual ActionResult Talk()
        {
            return View();
        }
    }
}