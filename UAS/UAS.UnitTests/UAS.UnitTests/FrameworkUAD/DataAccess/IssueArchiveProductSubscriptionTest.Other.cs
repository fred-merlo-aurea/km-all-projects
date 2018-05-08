using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Linq;
using FrameworkUAD.DataAccess;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Entity;
using ClientConnections = KMPlatform.Object.ClientConnections;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit Tests for <see cref="IssueArchiveProductSubscription"/>
    /// </summary>
    [TestFixture]
    public partial class IssueArchiveProductSubscriptionTest
    {
        private const int Rows = 5;
        private const int IssueId = 1;
        private const int Page = 3;
        private const int PageSize = 4;
        private const int ProductId = 8;
        private const string PubSubs = "pubsubs";
        private const string ProcSelectIssue = "e_IssueArchiveSubscription_Select_IssueID";
        private const string ProcSelectPaging = "e_IssueArchiveSubscription_Select_Paging";
        private const string ProcSelectCount = "e_IssueArchiveSubscription_Select_Count";
        private const string ProcSave = "e_IssueArchiveProductSubscription_Save";
        private const string ProcSelectForUpdate = "e_IssueArchiveProductSubscription_SelectForUpdate";
        private const string MethodGet = "Get";
        private static readonly ClientConnections Client = new ClientConnections();

        private DataTable _dataTable;
        private IList<Entity.IssueArchiveProductSubscription> _list;
        private Entity.IssueArchiveProductSubscription _objWithRandomValues;
        private Entity.IssueArchiveProductSubscription _objWithDefaultValues;
        private SqlCommand _sqlCommand;
        private Dictionary<string, string> _bulkCopyColumns;
        private bool _bulkCopyClosed;
        private List<Entity.IssueArchiveProductSubscription> _subscriptions;

        [Test]
        public void Get_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = typeof(IssueArchiveProductSubscription).CallMethod(MethodGet, new object[] { new SqlCommand() });

            // Assert
            result.ShouldBe(_objWithDefaultValues);
        }

        [Test]
        public void SelectIssue_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = IssueArchiveProductSubscription.SelectIssue(IssueId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@IssueID"].Value.ShouldBe(IssueId),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectIssue),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectPaging_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = IssueArchiveProductSubscription.SelectPaging(Page, PageSize, IssueId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@CurrentPage"].Value.ShouldBe(Page),
                () => _sqlCommand.Parameters["@PageSize"].Value.ShouldBe(PageSize),
                () => _sqlCommand.Parameters["@IssueID"].Value.ShouldBe(IssueId),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectPaging),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectCount_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = IssueArchiveProductSubscription.SelectCount(IssueId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@IssueID"].Value.ShouldBe(IssueId),
                () => result.ShouldBe(Rows),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectCount),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Save_WhenCalledWithRandomValues_VerifySqlParameters()
        {
            // Arrange, Act
            var result = IssueArchiveProductSubscription.Save(_objWithRandomValues, Client);

            // Assert
            result.ShouldBe(Rows);
            VerifySqlCommand(_objWithRandomValues);
        }

        [Test]
        public void Save_WhenCalledWithDefaultValues_VerifySqlParameters()
        {
            // Arrange, Act
            var result = IssueArchiveProductSubscription.Save(_objWithDefaultValues, Client);

            // Assert
            result.ShouldBe(Rows);
            VerifySqlCommand(_objWithDefaultValues);
        }

        private void VerifySqlCommand(Entity.IssueArchiveProductSubscription subscription)
        {
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@IssueArchiveSubscriptionId"].Value.ShouldBe(subscription.IssueArchiveSubscriptionId),
                () => _sqlCommand.Parameters["@IsComp"].Value.ShouldBe((object)subscription.IsComp ?? DBNull.Value),
                () => _sqlCommand.Parameters["@CompId"].Value.ShouldBe(subscription.CompId),
                () => _sqlCommand.Parameters["@PubSubscriptionID"].Value.ShouldBe(subscription.PubSubscriptionID),
                () => _sqlCommand.Parameters["@SubscriptionID"].Value.ShouldBe(subscription.SubscriptionID),
                () => _sqlCommand.Parameters["@PubID"].Value.ShouldBe(subscription.PubID),
                () => _sqlCommand.Parameters["@demo7"].Value.ShouldBe((object)subscription.Demo7 ?? DBNull.Value),
                () => _sqlCommand.Parameters["@Qualificationdate"].Value.ShouldBe((object)subscription.QualificationDate ?? DBNull.Value),
                () => _sqlCommand.Parameters["@PubQSourceID"].Value.ShouldBe(subscription.PubQSourceID),
                () => _sqlCommand.Parameters["@PubCategoryID"].Value.ShouldBe(subscription.PubCategoryID),
                () => _sqlCommand.Parameters["@PubTransactionID"].Value.ShouldBe(subscription.PubTransactionID),
                () => _sqlCommand.Parameters["@Email"].Value.ShouldBe((object)subscription.Email ?? DBNull.Value),
                () => _sqlCommand.Parameters["@EmailStatusID"].Value.ShouldBe(subscription.EmailStatusID),
                () => _sqlCommand.Parameters["@StatusUpdatedDate"].Value.ShouldBe(subscription.StatusUpdatedDate),
                () => _sqlCommand.Parameters["@StatusUpdatedReason"].Value.ShouldBe((object)subscription.StatusUpdatedReason ?? DBNull.Value),
                () => _sqlCommand.Parameters["@DateCreated"].Value.ShouldBe(subscription.DateCreated),
                () => _sqlCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)subscription.DateUpdated ?? DBNull.Value),
                () => _sqlCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(subscription.CreatedByUserID),
                () => _sqlCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)subscription.UpdatedByUserID ?? DBNull.Value),
                () => _sqlCommand.CommandText.ShouldBe(ProcSave),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SaveBulkSqlInsert_WhenCalled_VerifyReturnItem()
        {
            // Arrange
            SetupBulkCopyFakes();
            var expectedList = new List<Entity.IssueArchiveProductSubscription>
            {
                typeof(Entity.IssueArchiveProductSubscription).CreateInstance(),
                typeof(Entity.IssueArchiveProductSubscription).CreateInstance(true)
            };

            // Act
            var result = IssueArchiveProductSubscription.SaveBulkSqlInsert(expectedList, Client);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => _bulkCopyClosed.ShouldBeTrue(),
                () => result.ShouldBeTrue(),
                () => _bulkCopyColumns.Keys.ToArray().ShouldBe(Columns),
                () => _bulkCopyColumns.Values.ToArray().ShouldBe(Columns),
                () => _subscriptions.ShouldNotBeNull(),
                () => _subscriptions.IsListContentMatched(expectedList));
        }

        private static string[] Columns => new[]
        {
            "IssueArchiveSubscriptionId", "IssueArchiveSubscriberId", "IsComp", "CompId", "SubscriptionID",
            "SequenceID", "SubscriberID",  "PublisherID", "PublicationID", "ActionID_Current", "ActionID_Previous",
            "SubscriptionStatusID", "IsPaid", "QSourceID", "QSourceDate", "DeliverabilityID", "IsSubscribed",
            "SubscriberSourceCode", "Copies", "OriginalSubscriberSourceCode", "Par3cID", "SubsrcTypeID", "AccountNumber",
            "OnBehalfOf", "MemberGroup", "Verify", "DateCreated", "DateUpdated", "CreatedByUserID", "UpdatedByUserID"
        };

        [Test]
        public void SelectForUpdate_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = IssueArchiveProductSubscription.SelectForUpdate(ProductId, IssueId, PubSubs, Client);

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

        private void SetupBulkCopyFakes()
        {
            ShimSqlBulkCopy.AllInstances.WriteToServerDataTable = (bulkCopy, table) =>
            {
                foreach (SqlBulkCopyColumnMapping mapping in bulkCopy.ColumnMappings)
                {
                    _bulkCopyColumns.Add(mapping.SourceColumn, mapping.DestinationColumn);
                }

                _subscriptions = table.ConvertDataTable<Entity.IssueArchiveProductSubscription>();
            };

            ShimSqlBulkCopy.AllInstances.Close = _ => { _bulkCopyClosed = true; };
        }
    }
}