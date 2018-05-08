using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
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
    /// Unit Tests for <see cref="History"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class HistoryTests
    {
        private const int Rows = 5;
        private const int BatchId = 1;
        private const int UserId = 4;
        private const int PublicationId = 5;
        private const int HistoryId = 6;
        private const int UserLogId = 9;
        private const int HistoryMarketingMapId = 11;
        private const string ProcSelect = "e_History_Select";
        private const string ProcSelectBatchId = "e_History_Select_BatchID";
        private const string ProcSelectDate = "e_History_Select_DateCreated";
        private const string ProcSelectBatch = "e_History_Select_Active_User_BatchID";
        private const string ProcUserLogList = "e_HistoryToUserLog_HistoryID";
        private const string ProcHistoryResponseList = "e_HistoryToHistoryResonse_HistoryID";
        private const string ProcHistoryMarketingMapList = "e_HistoryToHistoryMarketingMap_HistoryID";
        private const string ProcInsertHistoryToUserLog = "e_HistoryToUserLog_Save";
        private const string ProcInsertHistoryToHistoryMarketingMapList = "e_HistoryToHistoryMarketingMap_BulkSave";
        private const string ProcInsertHistoryToHistoryMarketingMap = "e_HistoryToHistoryMarketingMap_Save";
        private const string GetMethod = "Get";
        private static readonly ClientConnections Client = new ClientConnections();

        private IDisposable _context;
        private DataTable _dataTable;
        private IList<Entity.History> _list;
        private Entity.History _objWithRandomValues;
        private Entity.History _objWithDefaultValues;
        private SqlCommand _sqlCommand;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _dataTable = new DataTable();

            _objWithRandomValues = typeof(Entity.History)
                .CreateInstance();
            _objWithDefaultValues = typeof(Entity.History)
                .CreateInstance(true);

            _list = new List<Entity.History>
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
        public void Select_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = History.Select(Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelect),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Select_WhenCalledWithBatchId_VerifySqlParameters()
        {
            // Arrange, Act
            var result = History.Select(BatchId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@BatchID"].Value.ShouldBe(BatchId),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectBatchId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Select_WhenCalledWithDates_VerifySqlParameters()
        {
            // Arrange
            var startDate = DateTime.Now;
            var endDate = DateTime.Now;

            // Act
            var result = History.Select(startDate, endDate, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@StartDate"].Value.ShouldBe(startDate),
                () => _sqlCommand.Parameters["@EndDate"].Value.ShouldBe(endDate),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectDate),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectBatch_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = History.SelectBatch(UserId, PublicationId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@UserID"].Value.ShouldBe(UserId),
                () => _sqlCommand.Parameters["@PublicationID"].Value.ShouldBe(PublicationId),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectBatch),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void UserLogList_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return new List<int> { 1 }.GetSqlDataReader();
            };

            // Act
            var result = History.UserLogList(HistoryId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@HistoryID"].Value.ShouldBe(HistoryId),
                () => result.ShouldBe(new List<int>()),
                () => _sqlCommand.CommandText.ShouldBe(ProcUserLogList),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void HistoryResponseList_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return new List<int> { 1 }.GetSqlDataReader();
            };

            // Act
            var result = History.HistoryResponseList(HistoryId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@HistoryID"].Value.ShouldBe(HistoryId),
                () => result.ShouldBe(new List<int>()),
                () => _sqlCommand.CommandText.ShouldBe(ProcHistoryResponseList),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void HistoryMarketingMapList_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return new List<int> { 1 }.GetSqlDataReader();
            };

            // Act
            var result = History.HistoryMarketingMapList(HistoryId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@HistoryID"].Value.ShouldBe(HistoryId),
                () => result.ShouldBe(new List<int>()),
                () => _sqlCommand.CommandText.ShouldBe(ProcHistoryMarketingMapList),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Get_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = typeof(History).CallMethod(GetMethod, new object[] { new SqlCommand() });

            // Assert
            result.ShouldBe(_objWithDefaultValues);
        }

        [Test]
        public void InsertHistoryToUserLog_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = History.Insert_History_To_UserLog(HistoryId, UserLogId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@HistoryID"].Value.ShouldBe(HistoryId),
                () => _sqlCommand.Parameters["@UserLogID"].Value.ShouldBe(UserLogId),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcInsertHistoryToUserLog),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void InsertHistoryToHistoryMarketingMap_List_WhenCalled_VerifySqlParameters()
        {
            // Arrange 
            var maps = new List<Entity.HistoryMarketingMap>
            {
                typeof(Entity.HistoryMarketingMap).CreateInstance()
            };

            StringBuilder xml = new StringBuilder();
            xml.AppendLine("<XML>");
            foreach (Entity.HistoryMarketingMap x in maps)
            {
                xml.AppendLine("<History>")
                    .AppendLine($"<HistoryID>{HistoryId}</HistoryID>")
                    .AppendLine($"<HistoryMarketingMapID>{x.HistoryMarketingMapID}</HistoryMarketingMapID>")
                    .AppendLine("</History>");
            }
            xml.AppendLine("</XML>");

            // Arrange, Act
            var result = History.Insert_History_To_HistoryMarketingMap_List(HistoryId, maps, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@xml"].Value.ShouldBe(xml.ToString()),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcInsertHistoryToHistoryMarketingMapList),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void InsertHistoryToHistoryMarketingMap_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = History.Insert_History_To_HistoryMarketingMap(HistoryId, HistoryMarketingMapId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@HistoryID"].Value.ShouldBe(HistoryId),
                () => _sqlCommand.Parameters["@HistoryMarketingMapID"].Value.ShouldBe(HistoryMarketingMapId),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcInsertHistoryToHistoryMarketingMap),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
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