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
    /// Unit tests for <see cref="SubscriptionPaid"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SubscriptionPaidTest
    {
        private const string CommandText = "e_SubscriptionPaid_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.SubscriptionPaid _paid;

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
            _paid = new Entity.SubscriptionPaid(typeof(Entity.SubscriptionPaid).CreateInstance());

            // Act
            SubscriptionPaid.Save(_paid, new ClientConnections());

            // Assert
            _paid.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _paid = typeof(Entity.SubscriptionPaid).CreateInstance(true);

            // Act
            SubscriptionPaid.Save(_paid, new ClientConnections());

            // Assert
            _paid.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@SubscriptionPaidID"].Value.ShouldBe(_paid.SubscriptionPaidID),
                () => _saveCommand.Parameters["@PubSubscriptionID"].Value.ShouldBe(_paid.PubSubscriptionID),
                () => _saveCommand.Parameters["@PriceCodeID"].Value.ShouldBe(_paid.PriceCodeID),
                () => _saveCommand.Parameters["@StartIssueDate"].Value.ShouldBe(_paid.StartIssueDate),
                () => _saveCommand.Parameters["@ExpireIssueDate"].Value.ShouldBe(_paid.ExpireIssueDate),
                () => _saveCommand.Parameters["@CPRate"].Value.ShouldBe(_paid.CPRate),
                () => _saveCommand.Parameters["@Amount"].Value.ShouldBe(_paid.Amount),
                () => _saveCommand.Parameters["@AmountPaid"].Value.ShouldBe(_paid.AmountPaid),
                () => _saveCommand.Parameters["@BalanceDue"].Value.ShouldBe(_paid.BalanceDue),
                () => _saveCommand.Parameters["@PaidDate"].Value.ShouldBe(_paid.PaidDate),
                () => _saveCommand.Parameters["@TotalIssues"].Value.ShouldBe(_paid.TotalIssues),
                () => _saveCommand.Parameters["@CheckNumber"].Value.ShouldBe((object)_paid.CheckNumber ?? DBNull.Value),
                () => _saveCommand.Parameters["@CCNumber"].Value.ShouldBe((object)_paid.CCNumber ?? DBNull.Value),
                () => _saveCommand.Parameters["@CCExpirationMonth"].Value.ShouldBe((object)_paid.CCExpirationMonth ?? DBNull.Value),
                () => _saveCommand.Parameters["@CCExpirationYear"].Value.ShouldBe((object)_paid.CCExpirationYear ?? DBNull.Value),
                () => _saveCommand.Parameters["@CCHolderName"].Value.ShouldBe((object)_paid.CCHolderName ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreditCardTypeID"].Value.ShouldBe(_paid.CreditCardTypeID),
                () => _saveCommand.Parameters["@PaymentTypeID"].Value.ShouldBe(_paid.PaymentTypeID),
                () => _saveCommand.Parameters["@DeliverID"].Value.ShouldBe(_paid.DeliverID),
                () => _saveCommand.Parameters["@GraceIssues"].Value.ShouldBe(_paid.GraceIssues),
                () => _saveCommand.Parameters["@WriteOffAmount"].Value.ShouldBe(_paid.WriteOffAmount),
                () => _saveCommand.Parameters["@OtherType"].Value.ShouldBe((object)_paid.OtherType ?? DBNull.Value),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_paid.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_paid.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_paid.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_paid.UpdatedByUserID ?? DBNull.Value),
                () => _saveCommand.Parameters["@Frequency"].Value.ShouldBe(_paid.Frequency),
                () => _saveCommand.Parameters["@Term"].Value.ShouldBe(_paid.Term));
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
