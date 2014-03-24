namespace Glimpse.Build
{
    public class Settings : ISettings
    {
        public IStatusQueryProvider StatusQueryProvider { get; private set; }

        public void Initialize()
        {
            StatusQueryProvider = new StatusQueryProvider();
        }
    }
}