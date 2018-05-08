using System.Data.SqlClient;

namespace ecn.activityengines.Tests.Setup.Interfaces
{
    public interface ISqlCommand
    {
        string CommandText { get; set; }
        int CommandTimeout { get; set; }
        SqlConnection Connection { get; set; }

        void Constructor(string commandText, SqlConnection connection);
        void AddParameter(SqlParameter result);
        int ExecuteNonQuery();
    }
}
