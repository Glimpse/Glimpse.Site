using System.Collections.Generic;
using System.Threading.Tasks;

namespace Glimpse.Blog
{
    public interface IPostQueryProvider
    {
        Task<List<BlogResult>> CurrentPosts();
    }
}