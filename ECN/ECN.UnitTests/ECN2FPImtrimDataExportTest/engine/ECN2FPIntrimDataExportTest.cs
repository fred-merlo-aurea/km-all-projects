using System;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.IO.Fakes;
using ecn.kmps.ECN2FPImtrimDataExport;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;

namespace ECN2FPImtrimDataExport.Tests.engine
{
    /// <summary>
    /// Unit tests for <see cref="ECN2FPIntrimDataExport"/>
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class Ecn2FpIntrimDataExportTest
    {
        private IDisposable _context;
        private ECN2FPIntrimDataExport _export;
        private string _message;
        private SqlCommand _insertCommand;
        private string _invalidValue;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _export = new ECN2FPIntrimDataExport();
            _invalidValue = string.Empty;

            SetupFakes();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        private void SetupFakes()
        {
            ShimConfigurationManager.AppSettingsGet = () => new NameValueCollection
            {
                [ConnString] = TestConnectionString
            };

            ShimSqlConnection.ConstructorString = (_, __) => { };

            ShimSqlConnection.AllInstances.Open = _ => {};
            ShimSqlCommand.AllInstances.ExecuteNonQuery = command =>
            {
                _insertCommand = command;
                return -1;
            };
            ShimSqlConnection.AllInstances.Close = _ => { };

            ShimStreamWriter.AllInstances.Flush = _ => { };
            ShimTextWriter.AllInstances.WriteLineString = (_, str) =>
            {
                _message = str;
            };
        }
    }
}
