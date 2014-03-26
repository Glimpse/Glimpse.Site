using System.Web.Mvc;
using Glimpse.Release;
using Glimpse.Site.Models;

namespace Glimpse.Site.Controllers
{
    public partial class StatusController : AsyncController
    {
        //[OutputCache(Duration = 30 * 60)]
        public virtual ActionResult Index()
        {
            var model = ReleaseSettings.Settings.ReleaseService.GetRelease("vNext");

            return View(model);
        }

        public virtual ActionResult InvalidateCacheForIndex()
        {
            Response.RemoveOutputCacheItem(Url.Action("index"));

            return RedirectToAction("Index");
        }
	}
}