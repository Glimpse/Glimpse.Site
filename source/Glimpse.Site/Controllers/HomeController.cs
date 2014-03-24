using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Glimpse.Site.Controllers
{
    public partial class HomeController : Controller
    {
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult GettingStarted(string task)
        {
            return View(task);
        }  

        [OutputCache(Duration = 3600)] // Cache for 1 hour
        public async virtual Task<ActionResult> BlogLatest()
        {
            var httpClient = new HttpClient();

            var response = await httpClient.GetStringAsync("http://feeds.getglimpse.com/getglimpse");
            var xml = XElement.Parse(response);
            var result = new List<object>();

            foreach (var item in xml.Descendants("item").Take(2))
            {
                result.Add(new
                {
                    title = item.Element("title").Value,
                    summary = item.Element("description").Value,
                    link = item.Element("link").Value
                });
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private static string GetBearerTokenCredentials(string key, string secret)
        {
            var encodedKey = HttpUtility.UrlEncode(key);
            var encodedSecret = HttpUtility.UrlEncode(secret);

            var concatenatedKeySecret = string.Format("{0}:{1}", encodedKey, encodedSecret);

            return Convert.ToBase64String(Encoding.ASCII.GetBytes(concatenatedKeySecret));
        }
    }
}