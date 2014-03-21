using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Glimpse.Package
{
    public class ConfigSectionGlimpse : ConfigurationSection
    {   

        [ConfigurationProperty("useOfflineData", DefaultValue = null)]
        public bool? UseOfflineData
        {
            get { return (bool?)base["useOfflineData"]; }
            set { base["useOfflineData"] = value; }
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
    }
}
