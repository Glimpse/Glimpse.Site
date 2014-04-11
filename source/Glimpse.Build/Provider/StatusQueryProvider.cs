using System;
using System.Globalization;
using System.Net.Http; 
using System.Threading.Tasks; 
using System.Xml.Linq;

namespace Glimpse.Build
{
    public class StatusQueryProvider : IStatusQueryProvider
    {
        public async Task<StatusResult> CurrentStatus()
        { 
            var xmlString = await new HttpClient().GetStringAsync("http://teamcity.codebetter.com/app/rest/builds/buildType:%28id:bt428%29?guest=1");
            var xml = XElement.Parse(xmlString);

            var date = ProcessData(xml.Element("startDate").Value);

            var result = new StatusResult
                {
                    Id = xml.Attribute("id").Value,
                    Number = xml.Attribute("number").Value,
                    Status = xml.Attribute("status").Value.ToLower(),
                    Link = xml.Attribute("webUrl").Value + "&guest=1",
                    Date = date.DateTime.ToShortDateString(),
                    Time = date.DateTime.ToShortTimeString()
                };

            return result;
        }

        private DateTimeOffset ProcessData(string value)
        {
            DateTimeOffset date;
            DateTimeOffset.TryParseExact(value, "yyyyMMddTHHmmsszz00", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            return date;
        }
    }
}