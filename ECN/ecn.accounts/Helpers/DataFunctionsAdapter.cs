using System.Data;
using Ecn.Accounts.Interfaces;
using ECN_Framework_DataLayer;

namespace Ecn.Accounts.Helpers
{
    public class DataFunctionsAdapter : IDataFunctions
    {
        public DataTable GetDataTable(string sql, string connectionStringName)
        {
            return DataFunctions.GetDataTable(sql, connectionStringName);
        }
    }
}
