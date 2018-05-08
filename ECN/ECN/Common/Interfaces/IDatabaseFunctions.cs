using System.Data;

namespace ECN.Common.Interfaces
{
    public interface IDatabaseFunctions
    {
        void OpenConnection(IDbCommand command);
        object ExecuteScalar(IDbCommand command);
        int ExecuteNonQuery(IDbCommand command);
        void CloseConnection(IDbConnection connection);
    }
}