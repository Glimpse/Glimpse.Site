using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Glimpse.Site.Controllers
{
    public partial class VersionController : Controller
    {
        public virtual ActionResult Install()
        {
            return View();
        }
        public virtual ActionResult Update()
        {
            return View();
        } 
    }
}
