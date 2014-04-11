using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glimpse.Contributor;

namespace Glimpse.Site.Controllers
{
    public partial class CommunityController : Controller
    {
        public virtual ActionResult Index()
        {
            var model = ContributorSettings.Settings.CommunityService.AllCommunity();

            return View(model);
        }

        public virtual ActionResult Talks()
        {
            return View();
        }
	}
}