using System.Data;
using System.Data.SqlClient;

namespace Ecn.Communicator.Main.Lists.Interfaces
{
    public interface IDatabaseAdapter
    {
        IDbConnection Connection { get; }
        IDbCommand CreateCommand();
        IDbCommand CreateCommand(string cmdText);
        IDbCommand CreateCommand(string cmdText, IDbConnection connection);
        void AddParameter(IDbCommand command, string parameterName, SqlDbType sqlDbType, object parameterValue);
    }
}
