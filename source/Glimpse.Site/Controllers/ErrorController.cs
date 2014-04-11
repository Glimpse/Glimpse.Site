using System.Net; 
using System.Web.Mvc;

namespace Glimpse.Site.Controllers
{
    public partial class ErrorController : Controller
    {
        //
        // GET: /Error/

        public virtual ActionResult ServerError()
        {
            SetStatusHeaders(HttpStatusCode.InternalServerError, "Internal Server Error");
            return View("500");
        }

        public virtual ActionResult NotFound()
        {
            SetStatusHeaders(HttpStatusCode.NotFound, "Not Found");
            return View("404");
        }

        public virtual ActionResult Unauthorized()
        {
            SetStatusHeaders(HttpStatusCode.Unauthorized, "Unauthorized");
            return View("401");
        }

        private void SetStatusHeaders(HttpStatusCode statusCode, string statusDescription)
        {
            Response.StatusCode = (int)statusCode;
            Response.StatusDescription = statusDescription;
        }
    }
}