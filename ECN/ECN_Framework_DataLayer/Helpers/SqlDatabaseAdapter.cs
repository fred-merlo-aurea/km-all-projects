using System.Data;
using System.Data.SqlClient;
using Ecn.Framework.DataLayer.Interfaces;

namespace Ecn.Framework.DataLayer.Helpers
{
    public class SqlDatabaseAdapter : IDatabaseAdapter
    {
        public void OpenConnection(IDbConnection connection)
        {
            connection.Open();
        }

        public IDbCommand CreateCommand(IDbConnection connection)
        {
            return connection.CreateCommand();
        }

        public IDbTransaction BeginTransaction(IDbConnection connection, string transactionName)
        {
            return (connection as SqlConnection).BeginTransaction(transactionName);
        }

        public int ExecuteNonQuery(IDbCommand command)
        {
            return command.ExecuteNonQuery();
        }

        public void CommitTransaction(IDbTransaction transaction)
        {
            transaction.Commit();
        }

        public void RollbackTransaction(IDbTransaction transaction)
        {
            transaction.Rollback();
        }

        public void AddParameterWithValue(IDbCommand command, string parameterName, object value)
        {
            (command as SqlCommand).Parameters.AddWithValue(parameterName, value);
        }

        public object ExecuteScalar(IDbCommand command)
        {
            return command.ExecuteScalar();
        }
    }
}
