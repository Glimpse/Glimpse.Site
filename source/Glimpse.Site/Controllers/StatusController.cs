using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Glimpse.Release;
using Glimpse.Site.Models;

namespace Glimpse.Site.Controllers
{
    public partial class StatusController : AsyncController
    {
        //[OutputCache(Duration = 30 * 60)]
        public virtual ActionResult Index(string milestone = null)
        {
            var model = new StatusViewModel
            {
                Release = ReleaseSettings.Settings.ReleaseService.GetRelease(milestone),
                Milestones = ReleaseSettings.Settings.MilestoneProvider.GetCurrentMilestones().Select(x => new SelectListItem { Selected = x.Title == milestone, Text = x.Title, Value = x.Title })
            };

            return View(model);
        }

        public virtual ActionResult InvalidateCacheForIndex()
        {
            Response.RemoveOutputCacheItem(Url.Action("index"));

            return RedirectToAction("Index");
        }
	}
}