using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using FrameworkUAD.Object;
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
    /// Unit Tests for <see cref="ArchivePubSubscriptionsExtension"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ArchivePubSubscriptionsExtensionTest
    {
        private const int ProductId = 2;
        private const int IssueId = 3;
        private const string PubSubs = "pub-subs";
        private const string ProcSelectForUpdate = "e_ArchivePubSubscriptionsExtension_SelectForUpdate";
        private const string ProcGetArchiveAdhocs = "o_ArchivePubSubscription_Adhocs";
        private const string ProcSave = "o_SaveArchivePubSubscriptions_AdHocs";
        private const int PubSubId = 5;
        private const int PubId = 6;
        private const int Id = 10;

        private static readonly ClientConnections Client = new ClientConnections();

        private IDisposable _context;
        private DataTable _dataTable;
        private SqlCommand _dataTableCommand;
        private SqlCommand _saveCommand;
        private IList<Entity.ArchivePubSubscriptionsExtension> _list;
        private Entity.ArchivePubSubscriptionsExtension _objWithRandomValues;
        private Entity.ArchivePubSubscriptionsExtension _objWithDefaultValues;
        private SqlCommand _getCommand;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _dataTable = new DataTable();

            _objWithRandomValues = typeof(Entity.ArchivePubSubscriptionsExtension).CreateInstance();
            _objWithDefaultValues = typeof(Entity.ArchivePubSubscriptionsExtension).CreateInstance(true);

            _list = new List<Entity.ArchivePubSubscriptionsExtension>
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
        public void SelectForUpdate_WhenCalled_VerifyListAndSqlParameters()
        {
            // Arrange, Act
            var actualList = ArchivePubSubscriptionsExtension.SelectForUpdate(ProductId, IssueId, PubSubs, Client);

            // Assert
            _getCommand.ShouldSatisfyAllConditions(
                () => _getCommand.Parameters["@ProductID"].Value.ShouldBe(ProductId),
                () => _getCommand.Parameters["@IssueID"].Value.ShouldBe(IssueId),
                () => _getCommand.Parameters["@PubSubs"].Value.ShouldBe(PubSubs),
                () => _getCommand.CommandText.ShouldBe(ProcSelectForUpdate),
                () => _getCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
                () => actualList.IsListContentMatched(_list.ToList()).ShouldBeTrue());
        }

        [Test]
        public void GetArchiveAdhocs_WhenCalled_VerifyDataTableAndSqlParameters()
        {
            // Arrange Act
            var actual = ArchivePubSubscriptionsExtension.GetArchiveAdhocs(Client, PubSubId, PubId, IssueId);

            // Assert
            _dataTableCommand.ShouldSatisfyAllConditions(
                () => _dataTableCommand.Parameters["@PubSubs"].Value.ShouldBe(PubSubId),
                () => _dataTableCommand.Parameters["@PubID"].Value.ShouldBe(PubId),
                () => _dataTableCommand.Parameters["@IssueID"].Value.ShouldBe(IssueId),
                () => _dataTableCommand.CommandText.ShouldBe(ProcGetArchiveAdhocs),
                () => _dataTableCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
                () => actual.ShouldBe(_dataTable));
        }

        [Test]
        public void Save_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var adhocs = new List<PubSubscriptionAdHoc>()
            {
                typeof(PubSubscriptionAdHoc).CreateInstance()
            };

            var xml =
                $"{adhocs.Aggregate("<XML>", (x, ah) => x + $"<AdHoc><Name>{ah.AdHocField}</Name><Value>{ah.Value}</Value></AdHoc>")}</XML>";

            // Act
            var actual = ArchivePubSubscriptionsExtension.Save(Client, Id, PubId, adhocs);

            // Assert
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@IssueArchiveSubscriptionID"].Value.ShouldBe(Id),
                () => _saveCommand.Parameters["@PubID"].Value.ShouldBe(PubId),
                () => _saveCommand.Parameters["@AdHocs"].Value.ShouldBe(xml),
                () => _saveCommand.CommandText.ShouldBe(ProcSave),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
                () => actual.ShouldBeTrue());
        }

        [Test]
        public void Get_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var actual = ArchivePubSubscriptionsExtension.Get(new SqlCommand());

            // Assert
            actual.ShouldBe(_objWithDefaultValues);
        }

        private void SetupFakes()
        {
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new ShimSqlConnection().Instance;
            KMFakes.ShimDataFunctions.GetDataTableViaAdapterSqlCommand = command =>
            {
                _dataTableCommand = command;
                return _dataTable;
            };

            KMFakes.ShimDataFunctions.ExecuteNonQuerySqlCommand = command =>
            {
                _saveCommand = command;
                return true;
            };

            ShimSqlCommand.AllInstances.ConnectionGet = command => new ShimSqlConnection().Instance;
            ShimDataFunctions.ExecuteReaderSqlCommand = command =>
            {
                _getCommand = command;
                return _list.GetSqlDataReader();
            };
        }
    }
}
