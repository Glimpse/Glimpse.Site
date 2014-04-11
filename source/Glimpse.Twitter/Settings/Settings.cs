namespace Glimpse.Twitter
{
    public class Settings : ISettings
    { 
        public ITweetQueryProvider TweetQueryProvider { get; private set; }

        public void Initialize()
        {
            TweetQueryProvider = new TweetQueryProvider();
        }
    }
}