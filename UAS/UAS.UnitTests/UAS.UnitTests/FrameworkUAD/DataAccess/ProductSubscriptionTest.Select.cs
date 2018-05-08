using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using FrameworkUAD.DataAccess.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Entity;
using ClientConnections = KMPlatform.Object.ClientConnections;
using ProductSubscription = FrameworkUAD.DataAccess.ProductSubscription;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    [TestFixture]
    public partial class ProductSubscriptionTest
    {
        private static readonly ClientConnections Client = new ClientConnections();

        private DataTable _dataTable;
        private IList<Entity.ProductSubscription> _list;
        private Entity.ProductSubscription _objWithRandomValues;
        private Entity.ProductSubscription _objWithDefaultValues;
        private SqlCommand _sqlCommand;

        [Test]
        public void Select_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ProductSubscription.Select(SubscriptionId, Client, ClientDisplayName);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@SubscriptionID"].Value.ShouldBe(SubscriptionId),
                () => _sqlCommand.Parameters["@ClientDisplayName"].Value.ShouldBe(ClientDisplayName),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelect),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Select_WhenCalledWithRecordIdentifier_VerifySqlParameters()
        {
            // Arrange
            var sfRecordIdentifier = new Guid();

            // Act
            var result = ProductSubscription.Select(sfRecordIdentifier, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@SFRecordIdentifier"].Value.ShouldBe(sfRecordIdentifier),
                () => result.ShouldBe(_objWithDefaultValues),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectSfRecodeIdentifier),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectSequenceIDPubID_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ProductSubscription.SelectSequenceIDPubID(SeqNum, PubId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@SequenceNum"].Value.ShouldBe(SeqNum),
                () => _sqlCommand.Parameters["@PubID"].Value.ShouldBe(PubId),
                () => result.ShouldBe(_objWithDefaultValues),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectSequenceIdPubId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectProductSubscription_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ProductSubscription.SelectProductSubscription(PubSubscriptionId, Client, ClientDisplayName);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@PubSubscriptionID"].Value.ShouldBe(PubSubscriptionId),
                () => _sqlCommand.Parameters["@ClientDisplayName"].Value.ShouldBe(ClientDisplayName),
                () => result.ShouldBe(_objWithDefaultValues),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectProductSubscription),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectProcessCode_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ProductSubscription.SelectProcessCode(ProcessCode, Client, ClientDisplayName);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProcessCode"].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.Parameters["@ClientDisplayName"].Value.ShouldBe(ClientDisplayName),
                () => result.ShouldBe(_objWithDefaultValues),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectProcessCode),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectPaging_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ProductSubscription.SelectPaging(Page, PageSize, ProductId, Client, ClientDisplayName);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@CurrentPage"].Value.ShouldBe(Page),
                () => _sqlCommand.Parameters["@PageSize"].Value.ShouldBe(PageSize),
                () => _sqlCommand.Parameters["@ProductID"].Value.ShouldBe(ProductId),
                () => _sqlCommand.Parameters["@ClientDisplayName"].Value.ShouldBe(ClientDisplayName),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectPaging),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectPublication_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ProductSubscription.SelectPublication(ProductId, Client, ClientDisplayName);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@PublicationID"].Value.ShouldBe(ProductId),
                () => _sqlCommand.Parameters["@ClientDisplayName"].Value.ShouldBe(ClientDisplayName),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectPublication),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectProductID_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var expectedList = new List<Entity.ActionProductSubscription>
            {
                typeof(Entity.ActionProductSubscription).CreateInstance()
            };
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return expectedList.GetSqlDataReader();
            };

            // Act
            var result = ProductSubscription.SelectProductID(PubId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProductID"].Value.ShouldBe(PubId),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(expectedList.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectProductId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectProductID_WhenCalledWithIssueId_VerifySqlParameters()
        {
            // Arrange
            var expectedList = new List<Entity.ActionProductSubscription>
            {
                typeof(Entity.ActionProductSubscription).CreateInstance()
            };
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return expectedList.GetSqlDataReader();
            };

            // Act
            var result = ProductSubscription.SelectProductID(PubId, IssueId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProductID"].Value.ShouldBe(PubId),
                () => _sqlCommand.Parameters["@IssueID"].Value.ShouldBe(IssueId),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(expectedList.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectProductIdIssueId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectAllActiveIDs_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var expectedList = new List<Entity.CopiesProductSubscription>
            {
                typeof(Entity.CopiesProductSubscription).CreateInstance()
            };
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return expectedList.GetSqlDataReader();
            };

            // Act
            var result = ProductSubscription.SelectAllActiveIDs(ProductId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProductID"].Value.ShouldBe(ProductId),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(expectedList.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectAllActiveIDs),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectSequence_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ProductSubscription.SelectSequence(SequenceId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@SequenceID"].Value.ShouldBe(SequenceId),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectSequence),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectSequence_WhenCalledWithWhereClause_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ProductSubscription.SelectSequence(SequenceIdWhereClause, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@SequenceIdWhereClause"].Value.ShouldBe(SequenceIdWhereClause),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectSequenceWhereClause),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Select_For_Export_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ProductSubscription.Select_For_Export(Page, PageSize, Columns, ProductId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@CurrentPage"].Value.ShouldBe(Page),
                () => _sqlCommand.Parameters["@PageSize"].Value.ShouldBe(PageSize),
                () => _sqlCommand.Parameters["@ProductID"].Value.ShouldBe(ProductId),
                () => _sqlCommand.Parameters["@Columns"].Value.ShouldBe(Columns),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectForExport),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectForExportStatic_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ProductSubscription.Select_For_Export_Static(ProductId, Cols, Subs, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProductID"].Value.ShouldBe(ProductId),
                () => _sqlCommand.Parameters["@Columns"].Value.ShouldBe(Cols),
                () => _sqlCommand.Parameters["@Subs"].Value.ShouldBe(Subs),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectPubSubscriptionsForExportStatic),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectForExportStatic_WhenCalledWithIssueId_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ProductSubscription.Select_For_Export_Static(ProductId, IssueId, Cols, Subs, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProductID"].Value.ShouldBe(ProductId),
                () => _sqlCommand.Parameters["@Columns"].Value.ShouldBe(Cols),
                () => _sqlCommand.Parameters["@Subs"].Value.ShouldBe(Subs),
                () => _sqlCommand.Parameters["@IssueID"].Value.ShouldBe(IssueId),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectIssueArchiveSubscriptionForExportStatic),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectForUpdate_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ProductSubscription.SelectForUpdate(ProductId, IssueId, PubSubs, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProductID"].Value.ShouldBe(ProductId),
                () => _sqlCommand.Parameters["@IssueID"].Value.ShouldBe(IssueId),
                () => _sqlCommand.Parameters["@PubSubs"].Value.ShouldBe(PubSubs),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectForUpdate),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectCount_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ProductSubscription.SelectCount(ProductId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProductID"].Value.ShouldBe(ProductId),
                () => result.ShouldBe(Rows),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectCount),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }
    }
}