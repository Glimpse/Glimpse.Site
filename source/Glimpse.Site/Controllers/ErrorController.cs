using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Glimpse.Package.Provider
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult ServerError()
        {
            SetStatusHeaders(HttpStatusCode.InternalServerError, "Internal Server Error");
            return View("500");
        }

        public ActionResult NotFound()
        {
            SetStatusHeaders(HttpStatusCode.NotFound, "Not Found");
            return View("404");
        }

        public ActionResult Unauthorized()
        {
            SetStatusHeaders(HttpStatusCode.Unauthorized, "Unauthorized");
            return View("401");
        }

        private void SetStatusHeaders(HttpStatusCode statusCode, string statusDescription)
        {
            Response.StatusCode = (int) statusCode;
            Response.StatusDescription = statusDescription;
        }
    }
}
