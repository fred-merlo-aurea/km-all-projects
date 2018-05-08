using System.Data;
using ECN.Common.Interfaces;

namespace ECN.Common.Helpers
{
    public class DatabaseFunctionsAdapter : IDatabaseFunctions
    {
        public void OpenConnection(IDbCommand command)
        {
            command.Connection.Open();
        }

        public object ExecuteScalar(IDbCommand command)
        {
            return command.ExecuteScalar();
        }

        public int ExecuteNonQuery(IDbCommand command)
        {
            return command.ExecuteNonQuery();
        }

        public void CloseConnection(IDbConnection connection)
        {
            connection.Close();
        }
    }
}