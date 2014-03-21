using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glimpse.Site.Framework;
using Glimpse.Site.Models;

namespace Glimpse.Site.Controllers
{
    public partial class StatusController : Controller
    {
        private readonly StatusViewModelMapper _statusDashboardProvider = new StatusViewModelMapper();

        [OutputCache(Duration = 30 * 60)]
        public virtual ActionResult Index()
        {
            var statusView = _statusDashboardProvider.SetupStatusDashboard(Server.MapPath("~/Content/packages.json"));
            return View(statusView);
        }

        public virtual ActionResult InvalidateCacheForIndex()
        {
            Response.RemoveOutputCacheItem(Url.Action("index"));
            return RedirectToAction("Index");
        }
	}
}