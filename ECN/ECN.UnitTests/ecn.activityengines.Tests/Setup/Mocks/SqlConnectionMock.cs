using System.Data.SqlClient;
using ecn.activityengines.Tests.Setup.Interfaces;
using Moq;
using Shim = System.Data.SqlClient.Fakes.ShimSqlConnection;

namespace ecn.activityengines.Tests.Setup.Mocks
{
    public class SqlConnectionMock : Mock<ISqlConnection>
    {
        public SqlConnectionMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            Shim.ConstructorString = Constructor;
            Shim.AllInstances.Open = Open;
            Shim.AllInstances.Close = Close;
        }

        private void Close(SqlConnection connection)
        {
            Object.Close();
        }

        private void Open(SqlConnection connection)
        {
            Object.Open();
        }

        private void Constructor(SqlConnection connection, string connectionString)
        {
            Object.Constructor(connectionString);
        }
    }
}
