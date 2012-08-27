using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Glimpse.VersionCheck
{
    public class ConfigSectionGlimpse : ConfigurationSection
    {  
        [ConfigurationProperty("debug", DefaultValue = false)]
        public bool? Debug
        {
            get { return (bool?)base["debug"]; }
            set { base["debug"] = value; }
        }

        [ConfigurationProperty("logging")]
        public ConfigElementLogging Logging
        {
            get { return (ConfigElementLogging)base["logging"]; }
            set { base["logging"] = value; }
        }

        [ConfigurationProperty("services")]
        public ConfigElementServices Services
        {
            get { return (ConfigElementServices)base["services"]; }
            set { base["services"] = value; }
        }

        [ConfigurationProperty("dataSource")]
        public ConfigElementDataSource DataSource
        {
            get { return (ConfigElementDataSource)base["dataSource"]; }
            set { base["dataSource"] = value; }
        }
    }
}
