using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Glimpse.Infrastructure;
using Glimpse.Infrastructure.GitHub;
using Glimpse.Infrastructure.Repositories;
using Glimpse.Site.Framework;
using Glimpse.Site.Models;

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
