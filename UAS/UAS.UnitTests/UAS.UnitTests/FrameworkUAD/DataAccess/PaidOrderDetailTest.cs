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
    /// Unit tests for <see cref="PaidOrderDetail"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class PaidOrderDetailTest
    {
        private const string CommandText = "e_PaidOrderDetail_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.PaidOrderDetail _paidOrderDetail;

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
            _paidOrderDetail = typeof(Entity.PaidOrderDetail).CreateInstance();

            // Act
            PaidOrderDetail.Save(_paidOrderDetail, new ClientConnections());

            // Assert
            _paidOrderDetail.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _paidOrderDetail = typeof(Entity.PaidOrderDetail).CreateInstance(true);

            // Act
            PaidOrderDetail.Save(_paidOrderDetail, new ClientConnections());

            // Assert
            _paidOrderDetail.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@PaidOrderDetailId"].Value.ShouldBe(_paidOrderDetail.PaidOrderDetailId),
                () => _saveCommand.Parameters["@PaidOrderId"].Value.ShouldBe(_paidOrderDetail.PaidOrderId),
                () => _saveCommand.Parameters["@ProductSubscriptionId"].Value.ShouldBe(_paidOrderDetail.ProductSubscriptionId),
                () => _saveCommand.Parameters["@ProductId"].Value.ShouldBe(_paidOrderDetail.ProductId),
                () => _saveCommand.Parameters["@RefundDate"].Value.ShouldBe((object)_paidOrderDetail.RefundDate ?? DBNull.Value),
                () => _saveCommand.Parameters["@FulfilledDate"].Value.ShouldBe((object)_paidOrderDetail.FulfilledDate ?? DBNull.Value),
                () => _saveCommand.Parameters["@SubTotal"].Value.ShouldBe(_paidOrderDetail.SubTotal),
                () => _saveCommand.Parameters["@TaxTotal"].Value.ShouldBe(_paidOrderDetail.TaxTotal),
                () => _saveCommand.Parameters["@GrandTotal"].Value.ShouldBe(_paidOrderDetail.GrandTotal),
                () => _saveCommand.Parameters["@SubGenBundleId"].Value.ShouldBe(_paidOrderDetail.SubGenBundleId),
                () => _saveCommand.Parameters["@SubGenOrderItemId"].Value.ShouldBe(_paidOrderDetail.SubGenOrderItemId),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_paidOrderDetail.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_paidOrderDetail.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserId"].Value.ShouldBe(_paidOrderDetail.CreatedByUserId),
                () => _saveCommand.Parameters["@UpdatedByUserId"].Value.ShouldBe((object)_paidOrderDetail.UpdatedByUserId ?? DBNull.Value));
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