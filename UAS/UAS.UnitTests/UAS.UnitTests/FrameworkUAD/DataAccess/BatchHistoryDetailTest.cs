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
using Entity = FrameworkUAD.Object;
using KMFakes = KM.Common.Fakes;
using ClientConnections = KMPlatform.Object.ClientConnections;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit Tests for <see cref="BatchHistoryDetail"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class BatchHistoryDetailTest
    {
        private const int Rows = 5;
        private const int UserId = 2;
        private const bool IsActive = true;
        private const string ClientName = "client-name";
        private const int BatchId = 4;
        private const int SubscriptionId = 9;
        private const int PubSubscriptionId = 6;
        private const string Name = "Name";
        private const int SequenceId = 8;
        private const string ProcSelectUserId = "o_BatchHistoryDetail_Select_UserID_IsActive";
        private const string ProcSelectUserIdBatchId = "o_BatchHistoryDetail_Select_UserID_IsActive_BatchID";
        private const string ProcSelectBatchId = "o_BatchHistoryDetail_Select_IsActive_BatchID";
        private const string ProcSelect = "o_BatchHistoryDetail_Select";
        private const string ProcSelectSubscriptionId = "o_BatchHistoryDetail_Select_SubscriptionID";
        private const string ProcSelectSubscriber = "o_BatchHistoryDetail_Select_SubscriberId";
        private const string ProcSelectBatch = "o_BatchHistoryDetail_Select_BatchID_Name_Sequence";
        private const string ProcSelectBatchDateRange = "o_BatchHistoryDetail_Select_BatchID_Name_Sequence_DateRange";
        private const string MethodGet = "Get";
        private static readonly ClientConnections Client = new ClientConnections();

        private IDisposable _context;
        private DataTable _dataTable;
        private IList<Entity.BatchHistoryDetail> _list;
        private Entity.BatchHistoryDetail _objWithRandomValues;
        private Entity.BatchHistoryDetail _objWithDefaultValues;
        private SqlCommand _sqlCommand;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _dataTable = new DataTable();

            _objWithRandomValues = typeof(Entity.BatchHistoryDetail)
                .CreateInstance();
            _objWithDefaultValues = typeof(Entity.BatchHistoryDetail)
                .CreateInstance(true);

            _list = new List<Entity.BatchHistoryDetail>
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
        public void Select_WhenCalledWithUserId_VerifySqlParameters()
        {
            // Arrange, Act
            var result = BatchHistoryDetail.Select(UserId, IsActive, Client, ClientName);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@UserID"].Value.ShouldBe(UserId),
                () => _sqlCommand.Parameters["@IsActive"].Value.ShouldBe(IsActive),
                () => _sqlCommand.Parameters["@ClientName"].Value.ShouldBe(ClientName),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectUserId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectBatchID_WhenCalledWithBatchId_VerifySqlParameters()
        {
            // Arrange, Act
            var result = BatchHistoryDetail.SelectBatchID(UserId, IsActive, Client, ClientName, BatchId);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@UserID"].Value.ShouldBe(UserId),
                () => _sqlCommand.Parameters["@IsActive"].Value.ShouldBe(IsActive),
                () => _sqlCommand.Parameters["@ClientName"].Value.ShouldBe(ClientName),
                () => _sqlCommand.Parameters["@BatchID"].Value.ShouldBe(BatchId),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectUserIdBatchId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectBatchID_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = BatchHistoryDetail.SelectBatchID(true, Client, ClientName, BatchId);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@IsActive"].Value.ShouldBe(IsActive),
                () => _sqlCommand.Parameters["@ClientName"].Value.ShouldBe(ClientName),
                () => _sqlCommand.Parameters["@BatchID"].Value.ShouldBe(BatchId),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectBatchId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Select_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = BatchHistoryDetail.Select(Client, ClientName);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ClientName"].Value.ShouldBe(ClientName),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelect),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Select_WhenCalledWithSubscriptionId_VerifySqlParameters()
        {
            // Arrange, Act
            var result = BatchHistoryDetail.Select(SubscriptionId, Client, ClientName);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@SubscriptionID"].Value.ShouldBe(SubscriptionId),
                () => _sqlCommand.Parameters["@ClientName"].Value.ShouldBe(ClientName),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectSubscriptionId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectSubscriber_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = BatchHistoryDetail.SelectSubscriber(PubSubscriptionId, Client, ClientName);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@PubSubscriptionID"].Value.ShouldBe(PubSubscriptionId),
                () => _sqlCommand.Parameters["@ClientName"].Value.ShouldBe(ClientName),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectSubscriber),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectBatch_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = BatchHistoryDetail.SelectBatch(BatchId, Name, SequenceId, Client, ClientName);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@BatchID"].Value.ShouldBe(BatchId),
                () => _sqlCommand.Parameters["@Name"].Value.ShouldBe(Name),
                () => _sqlCommand.Parameters["@SequenceID"].Value.ShouldBe(SequenceId),
                () => _sqlCommand.Parameters["@ClientName"].Value.ShouldBe(ClientName),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectBatch),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectBatch_WhenCalledWithDateRanges_VerifySqlParameters()
        {
            // Arrange
            var from = DateTime.Now;
            var to = DateTime.Now;

            // Act
            var result = BatchHistoryDetail.SelectBatch(BatchId, Name, SequenceId, from, to, Client, ClientName);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@BatchID"].Value.ShouldBe(BatchId),
                () => _sqlCommand.Parameters["@Name"].Value.ShouldBe(Name),
                () => _sqlCommand.Parameters["@SequenceID"].Value.ShouldBe(SequenceId),
                () => _sqlCommand.Parameters["@From"].Value.ShouldBe(from),
                () => _sqlCommand.Parameters["@To"].Value.ShouldBe(to),
                () => _sqlCommand.Parameters["@ClientName"].Value.ShouldBe(ClientName),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectBatchDateRange),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Get_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = typeof(BatchHistoryDetail)
                .CallMethod(MethodGet, new object[] { new SqlCommand() });

            // Assert
            result.ShouldBe(_objWithDefaultValues);
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