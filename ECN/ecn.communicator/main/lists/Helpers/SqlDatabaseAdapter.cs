using System.Data;
using System.Data.SqlClient;
using Ecn.Communicator.Main.Lists.Interfaces;

namespace Ecn.Communicator.Main.Lists.Helpers
{
    public class SqlDatabaseAdapter : IDatabaseAdapter
    {
        public IDbConnection Connection { get; }

        public SqlDatabaseAdapter(SqlConnection connection)
        {
            Connection = connection;
        }

        public IDbCommand CreateCommand()
        {
            return new SqlCommand();
        }

        public IDbCommand CreateCommand(string cmdText)
        {
            return new SqlCommand(cmdText);
        }

        public IDbCommand CreateCommand(string cmdText, IDbConnection connection)
        {
            return new SqlCommand(cmdText, connection as SqlConnection);
        }

        public void AddParameter(IDbCommand command, string parameterName, SqlDbType sqlDbType, object parameterValue)
        {
            (command as SqlCommand).Parameters.Add(parameterName, sqlDbType);
            (command as SqlCommand).Parameters[parameterName].Value = parameterValue;
        }
    }
}
