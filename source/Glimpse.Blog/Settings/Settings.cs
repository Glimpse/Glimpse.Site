namespace Glimpse.Blog
{
    public class Settings : ISettings
    {
        public IPostQueryProvider PostQueryProvider { get; private set; }

        public void Initialize()
        {
            PostQueryProvider = new PostQueryProvider();
        }
    }
}