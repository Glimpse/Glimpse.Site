namespace Glimpse.VersionCheck
{
    /// <summary>
    /// Singleton container for library settings.
    /// </summary>
    public static class GlimpseSettings
    {
        static GlimpseSettings()
        {
            Settings = new Settings();
        }

        /// <summary>
        /// Configuration settings for the system
        /// </summary>
        public static ISettings Settings { get; private set; }
    }
}