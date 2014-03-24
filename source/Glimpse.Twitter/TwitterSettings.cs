namespace Glimpse.Twitter
{
    /// <summary>
    /// Singleton container for library settings.
    /// </summary>
    public static class TwitterSettings
    {
        static TwitterSettings()
        {
            Settings = new Settings();
        }

        /// <summary>
        /// Configuration settings for the system
        /// </summary>
        public static ISettings Settings { get; private set; }
    }
}