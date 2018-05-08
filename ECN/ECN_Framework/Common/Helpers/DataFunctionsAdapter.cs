using ECN_Framework.Common.Interfaces;
using ECN_Framework_DataLayer;

namespace ECN_Framework.Common.Helpers
{
    public class DataFunctionsAdapter : IDataFunctions
    {
        public object ExecuteScalar(string sql, string connectionStringName)
        {
            return DataFunctions.ExecuteScalar(sql, connectionStringName);
        }
    }
}
