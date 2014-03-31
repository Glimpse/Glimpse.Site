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
            var release = ReleaseSettings.Settings.ReleaseService.GetRelease(milestone);

            var model = new StatusViewModel
            {
                Release = release,
                Milestones = ReleaseSettings.Settings.MilestoneProvider.GetCurrentMilestones().Select(x => new SelectListItem { Selected = release.Milestone != null && x.Title == release.Milestone.Title, Text = x.Title, Value = x.Title })
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