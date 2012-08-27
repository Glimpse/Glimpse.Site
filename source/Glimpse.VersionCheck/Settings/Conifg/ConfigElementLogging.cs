using System.Configuration;

namespace Glimpse.VersionCheck
{
    public class ConfigElementLogging : ConfigurationElement
    {
        [ConfigurationProperty("enabled", DefaultValue = null /*false*/)]
        public bool? Enabled
        {
            get { return (bool?)base["enabled"]; }
            set { base["enabled"] = value; }
        }

        [ConfigurationProperty("logEverything", DefaultValue = null /*false*/)]
        public bool? LogEverything
        {
            get { return (bool?)base["logEverything"]; }
            set { base["logEverything"] = value; }
        }

        [ConfigurationProperty("loggingPath", DefaultValue = null)]
        public string LoggingPath
        {
            get { return (string)base["loggingPath"]; }
            set { base["loggingPath"] = value; }
        }
    }
}