using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using ecn.activityengines.Tests.Setup.Interfaces;
using Moq;
using Shim = System.Data.SqlClient.Fakes.ShimSqlCommand;

namespace ecn.activityengines.Tests.Setup.Mocks
{
    public class SqlCommandMock : Mock<ISqlCommand>
    {
        private SqlConnection _connection;
        private List<SqlParameter> _sqlParametersList = new List<SqlParameter>();

        public IEnumerable<SqlParameter> SqlParameters => _sqlParametersList;

        public SqlCommandMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            Shim.ConstructorStringSqlConnection = Constructor;
            Shim.AllInstances.CommandTextSetString = CommandTextSet;
            Shim.AllInstances.CommandTimeoutSetInt32 = CommandTimeout;
            Shim.AllInstances.ConnectionGet = ConnectionGet;
            Shim.AllInstances.ConnectionSetSqlConnection = ConnectionSet;
            Shim.AllInstances.ExecuteNonQuery = ExecuteNonQuery;
            ShimSqlParameterCollection.AllInstances.AddStringSqlDbTypeInt32 = AddParameter;
            ShimSqlParameterCollection.AllInstances.AddStringSqlDbType = AddParameter;
        }

        private void ConnectionSet(SqlCommand command, SqlConnection connection)
        {
            Object.Connection = connection;
            _connection = connection;
        }

        private int ExecuteNonQuery(SqlCommand command)
        {
            return Object.ExecuteNonQuery();
        }

        private SqlConnection ConnectionGet(SqlCommand command)
        {
            return Object.Connection ?? _connection;
        }

        private void CommandTimeout(SqlCommand command, int timeout)
        {
            Object.CommandTimeout = timeout;
        }

        private SqlParameter AddParameter(
            SqlParameterCollection parameterCollection,
            string parameterName,
            SqlDbType sqlDbType,
            int size)
        {
            var result = new SqlParameter(parameterName, sqlDbType, size);
            _sqlParametersList.Add(result);
            Object.AddParameter(result);
            return result;
        }

        private SqlParameter AddParameter(
            SqlParameterCollection parameterCollection,
            string parameterName,
            SqlDbType sqlDbType)
        {
            var result = new SqlParameter(parameterName, sqlDbType);
            _sqlParametersList.Add(result);
            Object.AddParameter(result);
            return result;
        }

        private void CommandTextSet(SqlCommand command, string commandText)
        {
            Object.CommandText = commandText;
        }

        private void Constructor(SqlCommand command, string commandText, SqlConnection connection)
        {
            Object.Constructor(commandText, connection);
            _connection = connection;
        }
    }
}
