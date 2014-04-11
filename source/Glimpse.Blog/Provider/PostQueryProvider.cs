using System.Collections.Generic;
using System.Linq;
using System.Net.Http; 
using System.Threading.Tasks; 
using System.Xml.Linq;

namespace Glimpse.Blog
{
    public class PostQueryProvider : IPostQueryProvider
    {
        public async Task<List<BlogResult>> CurrentPosts()
        {
            var xmlString = await new HttpClient().GetStringAsync("http://feeds.getglimpse.com/getglimpse");
            var xml = XElement.Parse(xmlString);

            var result = new List<BlogResult>();
            foreach (var item in xml.Descendants("item").Take(2))
            {
                result.Add(new BlogResult
                {
                    Title = item.Element("title").Value,
                    Summary = item.Element("description").Value,
                    Link = item.Element("link").Value
                });
            }

            return result;
        } 
    }
}