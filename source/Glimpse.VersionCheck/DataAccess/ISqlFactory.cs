using System.Data.Common;

namespace Glimpse.VersionCheck
{
    public interface  ISqlFactory
    {
        DbConnection CreateDbConnection();

        DbConnection CreateDbConnection(string connectionName);
    }
}