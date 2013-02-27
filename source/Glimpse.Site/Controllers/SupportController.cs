using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Glimpse.Site.Controllers
{
    public partial class SupportController : Controller
    {
        public virtual ActionResult Index()
        {
            return View();
        }
    }
}
