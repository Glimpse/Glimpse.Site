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
        public async virtual Task<ActionResult> BuildLatest()
        {
            var httpClient = new HttpClient();

            var response = await httpClient.GetStringAsync("http://teamcity.codebetter.com/app/rest/builds/buildType:%28id:bt428%29?guest=1");
            var xml = XElement.Parse(response);

            DateTimeOffset date;
            DateTimeOffset.TryParseExact(xml.Element("startDate").Value, "yyyyMMddTHHmmsszz00", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

            return Json(
                new
                {
                    id = xml.Attribute("id").Value,
                    number = xml.Attribute("number").Value,
                    status = xml.Attribute("status").Value.ToLower(),
                    link = xml.Attribute("webUrl").Value + "&guest=1",
                    date = date.DateTime.ToShortDateString(),
                    time = date.DateTime.ToShortTimeString()
                },
                JsonRequestBehavior.AllowGet);
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