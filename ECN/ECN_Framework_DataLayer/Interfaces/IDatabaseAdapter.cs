using System.Data;

namespace Ecn.Framework.DataLayer.Interfaces
{
    public interface IDatabaseAdapter
    {
        void OpenConnection(IDbConnection connection);
        IDbCommand CreateCommand(IDbConnection connection);
        IDbTransaction BeginTransaction(IDbConnection connection, string transactionName);
        void AddParameterWithValue(IDbCommand command, string parameterName, object value);
        object ExecuteScalar(IDbCommand command);
        int ExecuteNonQuery(IDbCommand command);
        void CommitTransaction(IDbTransaction transaction);
        void RollbackTransaction(IDbTransaction transaction);
    }
}
