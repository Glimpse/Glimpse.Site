using System.Web.Mvc;

namespace Glimpse.Site.Controllers
{
    public class StatusController : Controller
    {
        private readonly StatusDashboardProvider _statusDashboardProvider = new StatusDashboardProvider();

        [OutputCache(Duration = 30 * 60)]
        public ActionResult Index()
        {
            var statusView = _statusDashboardProvider.SetupStatusDashboard(Server.MapPath("~/Content/packages.json"));
            return View(statusView);
        }

        public ActionResult InvalidateCacheForIndex()
        {
            Response.RemoveOutputCacheItem(Url.Action("index"));
            return RedirectToAction("Index");
        }
    }
}
