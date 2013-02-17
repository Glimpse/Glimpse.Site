namespace Glimpse.Package
{
    /// <summary>
    /// Singleton container for library settings.
    /// </summary>
    public static class PackageSettings
    {
        static PackageSettings()
        {
            Settings = new Settings();
        }

        /// <summary>
        /// Configuration settings for the system
        /// </summary>
        public static ISettings Settings { get; private set; }
    }
}