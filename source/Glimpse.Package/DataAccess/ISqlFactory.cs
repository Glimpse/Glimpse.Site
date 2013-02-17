using System.Data.Common;

namespace Glimpse.Package
{
    public interface  ISqlFactory
    {
        DbConnection CreateDbConnection();

        DbConnection CreateDbConnection(string connectionName);
    }
}