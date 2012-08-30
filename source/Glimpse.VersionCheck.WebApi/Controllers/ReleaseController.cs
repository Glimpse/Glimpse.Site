﻿using System;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace Glimpse.VersionCheck.WebApi.Controllers
{
    public class ReleaseController : Controller
    {
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Check(VersionCheckDetails details, bool withDetails)
        {
            var service = GlimpseSettings.Settings.NewReleaseService;
            var result = service.GetLatestReleaseInfo(details, withDetails);

            // Indicates how much data we want to show
            ViewBag.WithDetails = withDetails;

            return View(result);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Release(string package, string version, string stamp)
        {
            var service = GlimpseSettings.Settings.ReleaseService;
            var result = service.GetReleaseInfo(package, version);

            return View(result);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Test()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Update()
        {
            var service = GlimpseSettings.Settings.UpdateReleaseService;
            var results = service.Execute();

            return View(results);
        }
    }
}
