using System.Configuration;

namespace Glimpse.VersionCheck
{
    public class ConfigElementDataSource : ConfigurationElement
    {
        [ConfigurationProperty("disabledAutoBuild", DefaultValue = true)]
        public bool? DisabledAutoBuild
        {
            get { return (bool?)base["disabledAutoBuild"]; }
            set { base["disabledAutoBuild"] = value; }
        }
    }
}