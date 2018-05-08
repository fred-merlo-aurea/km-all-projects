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
using KMFakes = KM.Common.Fakes;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit tests for <see cref="IssueArchiveProductSubscriptionDetail"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class IssueArchiveProductSubscriptionDetailTest
    {
        private const string CommandText = "e_IssueArchiveProductSubscriptionDetail_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.IssueArchiveProductSubscriptionDetail _issueArchiveProductSubscriptionDetail;

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
            _issueArchiveProductSubscriptionDetail = typeof(Entity.IssueArchiveProductSubscriptionDetail).CreateInstance();

            // Act
            IssueArchiveProductSubscriptionDetail.Save(_issueArchiveProductSubscriptionDetail, new ClientConnections());

            // Assert
            _issueArchiveProductSubscriptionDetail.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _issueArchiveProductSubscriptionDetail = typeof(Entity.IssueArchiveProductSubscriptionDetail).CreateInstance(true);

            // Act
            IssueArchiveProductSubscriptionDetail.Save(_issueArchiveProductSubscriptionDetail, new ClientConnections());

            // Assert
            _issueArchiveProductSubscriptionDetail.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@IAProductSubscriptionDetailID"].Value.ShouldBe(_issueArchiveProductSubscriptionDetail.IAProductSubscriptionDetailID),
                () => _saveCommand.Parameters["@IssueArchiveSubscriptionId"].Value.ShouldBe(_issueArchiveProductSubscriptionDetail.IssueArchiveSubscriptionId),
                () => _saveCommand.Parameters["@SubscriptionID"].Value.ShouldBe(_issueArchiveProductSubscriptionDetail.SubscriptionID),
                () => _saveCommand.Parameters["@ResponseID"].Value.ShouldBe(_issueArchiveProductSubscriptionDetail.CodeSheetID),
                () => _saveCommand.Parameters["@ResponseOther"].Value.ShouldBe(_issueArchiveProductSubscriptionDetail.ResponseOther),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_issueArchiveProductSubscriptionDetail.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_issueArchiveProductSubscriptionDetail.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_issueArchiveProductSubscriptionDetail.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_issueArchiveProductSubscriptionDetail.UpdatedByUserID ?? DBNull.Value));
        }

        private void SetupFakes()
        {
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new ShimSqlConnection().Instance;
            KMFakes.ShimDataFunctions.ExecuteNonQuerySqlCommand = cmd =>
            {
                _saveCommand = cmd;
                return true;
            };
        }
    }
}