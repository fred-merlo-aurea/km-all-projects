using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Entity;
using KMFakes = KM.Common.Fakes;
using ClientConnections = KMPlatform.Object.ClientConnections;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit tests for <see cref="Report"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class ReportTest
    {
        private const string CommandText = "e_Reports_Save";

        private IDisposable _context;
        private Entity.Report _report;

        private static readonly ClientConnections Client = new ClientConnections();

        private DataTable _dataTable;
        private IList<Entity.Report> _list;
        private Entity.Report _objWithRandomValues;
        private Entity.Report _objWithDefaultValues;
        private SqlCommand _sqlCommand;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _dataTable = new DataTable();

            _objWithRandomValues = typeof(Entity.Report).CreateInstance();
            _objWithDefaultValues = typeof(Entity.Report).CreateInstance(true);

            _list = new List<Entity.Report>
            {
                _objWithRandomValues,
                _objWithDefaultValues
            };
            SetupFakes();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void Save_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            _report = typeof(Entity.Report).CreateInstance();

            // Act
            Report.Save(new ClientConnections(), _report);

            // Assert
            _report.ShouldSatisfyAllConditions(
                () => _sqlCommand.CommandText.ShouldBe(CommandText),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _report = typeof(Entity.Report).CreateInstance(true);

            // Act
            Report.Save(new ClientConnections(), _report);

            // Assert
            _report.ShouldSatisfyAllConditions(
                () => _sqlCommand.CommandText.ShouldBe(CommandText),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ReportID"].Value.ShouldBe(_report.ReportID),
                () => _sqlCommand.Parameters["@ReportName"].Value.ShouldBe(_report.ReportName),
                () => _sqlCommand.Parameters["@IsActive"].Value.ShouldBe(_report.IsActive),
                () => _sqlCommand.Parameters["@DateCreated"].Value.ShouldBe(_report.DateCreated),
                () => _sqlCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_report.DateUpdated ?? DBNull.Value),
                () => _sqlCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_report.CreatedByUserID),
                () => _sqlCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_report.UpdatedByUserID ?? DBNull.Value),
                () => _sqlCommand.Parameters["@ProvideID"].Value.ShouldBe(_report.IsActive),
                () => _sqlCommand.Parameters["@ProductID"].Value.ShouldBe((object)_report.ProductID ?? DBNull.Value),
                () => _sqlCommand.Parameters["@URL"].Value.ShouldBe((object)_report.URL ?? DBNull.Value),
                () => _sqlCommand.Parameters["@IsCrossTabReport"].Value.ShouldBe((object)_report.IsCrossTabReport ?? DBNull.Value),
                () => _sqlCommand.Parameters["@Row"].Value.ShouldBe((object)_report.Row ?? DBNull.Value),
                () => _sqlCommand.Parameters["@Column"].Value.ShouldBe((object)_report.Column ?? DBNull.Value),
                () => _sqlCommand.Parameters["@SuppressTotal"].Value.ShouldBe((object)_report.SuppressTotal ?? DBNull.Value),
                () => _sqlCommand.Parameters["@Status"].Value.ShouldBe((object)_report.Status ?? DBNull.Value),
                () => _sqlCommand.Parameters["@ReportTypeID"].Value.ShouldBe(_report.ReportTypeID));
        }

        private void SetupFakes()
        {
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new ShimSqlConnection().Instance;
            ShimDataFunctions.ExecuteScalarSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return Rows;
            };

            KMFakes.ShimDataFunctions.GetDataTableViaAdapterSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return _dataTable;
            };

            KMFakes.ShimDataFunctions.ExecuteNonQuerySqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return true;
            };

            ShimSqlCommand.AllInstances.ConnectionGet = cmd => new ShimSqlConnection().Instance;
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return _list.GetSqlDataReader();
            };
        }
    }
}