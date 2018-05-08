using System;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using ECN_Framework_DataLayer.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;

namespace ECN.Framework.Tests.Accounts.Report
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public abstract class ReportBaseTest
    {
        private IDisposable _shimObject;
        protected int _counter;
        protected int MaxRowCount;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();

            ShimSqlDataReader.AllInstances.Read =
            (_) =>
            {
                _counter++;
                return _counter <= MaxRowCount;
            };

            ShimSqlConnection.AllInstances.Close = (_) => { };
            ShimSqlConnection.AllInstances.DisposeBoolean = (_, __) => { };
            ShimSqlCommand.AllInstances.ConnectionGet = (_) => new ShimSqlConnection();
            ShimDataFunctions.ExecuteReaderSqlCommandString = (_, __) => new ShimSqlDataReader();
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }
    }
}
