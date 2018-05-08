using System.Data;

namespace Ecn.Accounts.Interfaces
{
    public interface IDataFunctions
    {
        DataTable GetDataTable(string sql, string connectionStringName);
    }
}
