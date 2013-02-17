using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Glimpse.Package
{
    public class SqlFactory : ISqlFactory
    {
        public DbConnection CreateDbConnection()
        {
            return CreateDbConnection("GlimpseConnection");
        }

        public DbConnection CreateDbConnection(string connectionName)
        {
            var key = ConfigurationManager.AppSettings[connectionName];
            var connectionDetails = WebConfigurationManager.ConnectionStrings[key];

            var factory = DbProviderFactories.GetFactory(connectionDetails.ProviderName);

            var connection = factory.CreateConnection();
            connection.ConnectionString = connectionDetails.ConnectionString;

            return connection;
        }
    }
}
