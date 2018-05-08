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
    /// Unit tests for <see cref="PaidOrder"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class PaidOrderTest
    {
        private const string CommandText = "e_PaidOrder_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.PaidOrder _order;

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
            _order = typeof(Entity.PaidOrder).CreateInstance();

            // Act
            PaidOrder.Save(_order, new ClientConnections());

            // Assert
            _order.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _order = typeof(Entity.PaidOrder).CreateInstance(true);

            // Act
            PaidOrder.Save(_order, new ClientConnections());

            // Assert
            _order.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@PaidOrderId"].Value.ShouldBe(_order.PaidOrderId),
                () => _saveCommand.Parameters["@SubscriptionId"].Value.ShouldBe(_order.SubscriptionId),
                () => _saveCommand.Parameters["@ImportName"].Value.ShouldBe(_order.ImportName),
                () => _saveCommand.Parameters["@OrderDate"].Value.ShouldBe(_order.OrderDate),
                () => _saveCommand.Parameters["@IsGift"].Value.ShouldBe(_order.IsGift),
                () => _saveCommand.Parameters["@SubTotal"].Value.ShouldBe(_order.SubTotal),
                () => _saveCommand.Parameters["@TaxTotal"].Value.ShouldBe(_order.TaxTotal),
                () => _saveCommand.Parameters["@GrandTotal"].Value.ShouldBe(_order.GrandTotal),
                () => _saveCommand.Parameters["@PaymentAmount"].Value.ShouldBe(_order.PaymentAmount),
                () => _saveCommand.Parameters["@PaymentNote"].Value.ShouldBe(_order.PaymentNote),
                () => _saveCommand.Parameters["@PaymentTransactionId"].Value.ShouldBe(_order.PaymentTransactionId),
                () => _saveCommand.Parameters["@PaymentTypeCodeId"].Value.ShouldBe(_order.PaymentTypeCodeId),
                () => _saveCommand.Parameters["@UserId"].Value.ShouldBe(_order.UserId),
                () => _saveCommand.Parameters["@SubGenOrderId"].Value.ShouldBe(_order.SubGenOrderId),
                () => _saveCommand.Parameters["@SubGenSubscriberId"].Value.ShouldBe(_order.SubGenSubscriberId),
                () => _saveCommand.Parameters["@SubGenUserId"].Value.ShouldBe(_order.SubGenUserId),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_order.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_order.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserId"].Value.ShouldBe(_order.CreatedByUserId),
                () => _saveCommand.Parameters["@UpdatedByUserId"].Value.ShouldBe((object)_order.UpdatedByUserId ?? DBNull.Value));
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
