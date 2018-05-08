using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using KMPlatform.Object;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Entity;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit tests for <see cref="FilterScheduleLog"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FilterScheduleLogTest
    {
        private const string CommandText = "e_FilterScheduleLog_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.FilterScheduleLog _filterScheduleLog;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
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
            _filterScheduleLog = typeof(Entity.FilterScheduleLog).CreateInstance();

            // Act
            FilterScheduleLog.Save(new ClientConnections(), _filterScheduleLog);

            // Assert
            _filterScheduleLog.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _filterScheduleLog = typeof(Entity.FilterScheduleLog).CreateInstance(true);

            // Act
            FilterScheduleLog.Save(new ClientConnections(), _filterScheduleLog);

            // Assert
            _filterScheduleLog.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@FilterScheduleID"].Value.ShouldBe(_filterScheduleLog.FilterScheduleID),
                () => _saveCommand.Parameters["@StartDate"].Value.ShouldBe(_filterScheduleLog.StartDate),
                () => _saveCommand.Parameters["@StartTime"].Value.ShouldBe(_filterScheduleLog.StartTime),
                () => _saveCommand.Parameters["@FileName"].Value.ShouldBe(_filterScheduleLog.FileName),
                () => _saveCommand.Parameters["@DownloadCount"].Value.ShouldBe(_filterScheduleLog.DownloadCount));
        }

        private void SetupFakes()
        {
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new ShimSqlConnection().Instance;
            ShimDataFunctions.ExecuteScalarSqlCommand = cmd =>
            {
                _saveCommand = cmd;
                return -1;
            };
        }
    }
}