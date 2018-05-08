using System;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using ECN_Framework_DataLayer.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;

namespace ECN.Framework.DataLayer.Tests.Communicator.Common
{
    [ExcludeFromCodeCoverage]
    public class Fakes
    {
        protected SqlParameterCollection ParameterCollection;
        protected int _maxRowCount;
        protected int _counter = 0;
        private IDisposable _context;

        public void SetupFakes()
        {
            _context = ShimsContext.Create();
            ParameterCollection = null;
            ShimForSqlConnection();
            ShimForSqlCommand(1);
            ShimDataFunctions.GetSqlConnectionString = s => new SqlConnection();
            ShimForSql();
        }

        [TearDown]
        public void DisposeContext()
        {
            _context.Dispose();
        }

        protected virtual string GetParameterValue(string key)
        {
            return ParameterCollection.Contains(key) ? ParameterCollection[key].Value.ToString() : null;
        }

        protected virtual void ShimForSqlConnection()
        {
            ShimSqlConnection.AllInstances.Open = connection => { };
        }

        protected virtual void ShimForCloseSqlConnection()
        {
            ShimSqlConnection.AllInstances.Close = connection => { };
        }

        protected virtual void ShimForSqlCommand(int affectedRows)
        {
            ShimSqlCommand.AllInstances.ExecuteNonQuery = command => affectedRows;
        }

        protected virtual void ShimForSql()
        {
            ShimDataFunctions.ExecuteReaderSqlCommandString = (_, __) => new ShimSqlDataReader();
            ShimSqlConnection.AllInstances.Close = (_) => { };
            ShimSqlConnection.AllInstances.DisposeBoolean = (_, __) => { };
            ShimSqlCommand.AllInstances.ConnectionGet = (_) => new ShimSqlConnection();
            ShimSqlDataReader.AllInstances.Read =
            (_) =>
            {
                _counter++;
                return _counter <= _maxRowCount;
            };
        }
    }
}
