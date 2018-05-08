
namespace ECN_Framework.Common.Interfaces
{
    public interface IDataFunctions
    {
        object ExecuteScalar(string sql, string connectionStringName);
    }
}
