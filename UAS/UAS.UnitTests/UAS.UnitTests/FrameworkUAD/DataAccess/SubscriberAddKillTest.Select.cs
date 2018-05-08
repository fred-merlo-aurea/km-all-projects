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
    /// Unit Tests for <see cref="SubscriberAddKill"/>
    /// </summary>
    public partial class SubscriberAddKillTest
    {
        private const int Rows = 5;
        private const int AddKillId = 2;
        private const int ProductId = 3;
        private const string SubscriptionIds = "subscription-ids";
        private const bool DeleteAddRemoveId = true;
        private const int AddRemoveId = 8;
        private const string ProcSelect = "e_SubscriberAddKill_Select";
        private const string ProcUpdateSubscription = "e_SubscriberAddKill_UpdateSubscription";
        private const string ProcClearDetails = "e_SubscriberAddKillDetail_Clear";
        private static readonly ClientConnections Client = new ClientConnections();

        private DataTable _dataTable;
        private IList<Entity.SubscriberAddKill> _list;
        private IList<Entity.SubscriberAddKillDetail> _detailList;
        private Dictionary<string, string> _bulkCopyColumns;
        private bool _bulkCopyClosed;
        private Entity.SubscriberAddKill _objWithRandomValues;
        private Entity.SubscriberAddKill _objWithDefaultValues;
        private SqlCommand _sqlCommand;

        [Test]
        public void Select_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberAddKill.Select(Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelect),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void UpdateSubscription_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberAddKill.UpdateSubscription(AddKillId, ProductId, SubscriptionIds, true, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@SubscriptionIDs"].Value.ShouldBe(SubscriptionIds),
                () => _sqlCommand.Parameters["@ProductID"].Value.ShouldBe(ProductId),
                () => _sqlCommand.Parameters["@AddRemoveID"].Value.ShouldBe(AddKillId),
                () => _sqlCommand.Parameters["@DeleteAddRemoveID"].Value.ShouldBe(DeleteAddRemoveId),
                () => result.ShouldBe(Rows),
                () => _sqlCommand.CommandText.ShouldBe(ProcUpdateSubscription),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void BulkInsertDetail_WhenCalled_VerifyReturnItem()
        {
            // Arrange
            SetupBulkCopyFakes();
            var expectedList = new List<Entity.SubscriberAddKillDetail>
            {
                typeof(Entity.SubscriberAddKillDetail).CreateInstance(),
                typeof(Entity.SubscriberAddKillDetail).CreateInstance(true)
            };

            // Act
            var result = SubscriberAddKill.BulkInsertDetail(expectedList, AddRemoveId, Client);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => _bulkCopyClosed.ShouldBeTrue(),
                () => _bulkCopyColumns.Keys.ToArray().ShouldBe(Columns),
                () => _bulkCopyColumns.Values.ToArray().ShouldBe(Columns),
                () => _detailList.ToList().IsListContentMatched(expectedList));
        }

        [Test]
        public void ClearDetails_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberAddKill.ClearDetails(ProductId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProductID"].Value.ShouldBe(ProductId),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcClearDetails),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        private static string[] Columns => new[]
        {
            "PubSubscriptionID",
            "AddKillID",
            "PubCategoryID",
            "PubTransactionID"
        };

        private void SetupBulkCopyFakes()
        {
            ShimSqlBulkCopy.AllInstances.WriteToServerDataTable = (bulkCopy, table) =>
            {
                foreach (SqlBulkCopyColumnMapping mapping in bulkCopy.ColumnMappings)
                {
                    _bulkCopyColumns.Add(mapping.SourceColumn, mapping.DestinationColumn);
                }

                _detailList = table.ConvertDataTable<Entity.SubscriberAddKillDetail>();
            };

            ShimSqlBulkCopy.AllInstances.Close = bulkCopy => { _bulkCopyClosed = true; };
        }
    }
}