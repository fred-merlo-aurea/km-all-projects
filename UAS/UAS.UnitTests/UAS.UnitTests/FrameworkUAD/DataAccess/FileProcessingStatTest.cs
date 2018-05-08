using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Entity;
using UASEntity = FrameworkUAS.Entity;
using KMFakes = KM.Common.Fakes;
using ClientConnections = KMPlatform.Object.ClientConnections;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit Tests for <see cref="FileProcessingStat"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FileProcessingStatTest
    {
        private const int Rows = 5;
        private const int ClientId = 5;
        private const string ProcNightlyInsert = "e_FileProcessingStat_NightlyInsert";
        private const string ProcSelect = "e_FileProcessingStat_Select_ProcessDate";
        private const string ProcSelectDateRange = "e_FileProcessingStat_Select_DateRange";
        private const string ProcGetFileProcessingStats = "job_GetFileProcessingStats";

        private static readonly ClientConnections Client = new ClientConnections();

        private IDisposable _context;
        private DataTable _dataTable;
        private IList<Entity.FileProcessingStat> _list;
        private Entity.FileProcessingStat _objWithRandomValues;
        private Entity.FileProcessingStat _objWithDefaultValues;
        private SqlCommand _sqlCommand;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _dataTable = new DataTable();

            _objWithRandomValues = typeof(Entity.FileProcessingStat).CreateInstance();
            _objWithDefaultValues = typeof(Entity.FileProcessingStat).CreateInstance(true);

            _list = new List<Entity.FileProcessingStat>
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
        public void NightlyInsert_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var processDate = DateTime.Now;

            // Act
            var result = FileProcessingStat.NightlyInsert(processDate, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProcessDate"].Value.ShouldBe(processDate),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcNightlyInsert),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Select_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var processDate = DateTime.Now;

            // Act
            var result = FileProcessingStat.Select(processDate, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProcessDate"].Value.ShouldBe(processDate),
                () => result.ShouldBe(_objWithDefaultValues),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelect),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectDateRange_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var startDate = DateTime.Now;
            var endDate = DateTime.Now;

            // Act
            var result = FileProcessingStat.SelectDateRange(startDate, endDate, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@StartDate"].Value.ShouldBe(startDate),
                () => _sqlCommand.Parameters["@EndDate"].Value.ShouldBe(endDate),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectDateRange),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Get_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = FileProcessingStat.Get(new SqlCommand());

            // Assert
            result.ShouldBe(_objWithDefaultValues);
        }

        [Test]
        public void GetList_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = FileProcessingStat.GetList(new SqlCommand());

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue());
        }

        [Test]
        public void GetFileProcessingStats_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var processDate = DateTime.Now;
            var fileProcessingStat = new UASEntity.FileProcessingStat();
            var expectedList = new List<UASEntity.FileProcessingStat>
            {
                fileProcessingStat
            };
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return expectedList.GetSqlDataReader();
            };

            // Act
            var result = FileProcessingStat.GetFileProcessingStats(processDate, ClientId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProcessDate"].Value.ShouldBe(processDate),
                () => result.ShouldBe(fileProcessingStat),
                () => _sqlCommand.CommandText.ShouldBe(ProcGetFileProcessingStats),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetListUAS_WhenCalled_VerifyReturnItem()
        {
            // Arrange
            var expectedList = new List<UASEntity.FileProcessingStat>
            {
                typeof(UASEntity.FileProcessingStat).CreateInstance(),
                typeof(UASEntity.FileProcessingStat).CreateInstance(true)
            };
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return expectedList.GetSqlDataReader();
            };

            // Act
            var result = FileProcessingStat.GetList(new SqlCommand(), ClientId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(expectedList.ToList()).ShouldBeTrue());
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